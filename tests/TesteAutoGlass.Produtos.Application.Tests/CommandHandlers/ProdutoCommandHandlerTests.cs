using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using TesteAutoGlass.Infraestruture.Data.Context.Abstraction;
using TesteAutoGlass.Infraestruture.Data.Repository.Produtos.Abstraction;
using TesteAutoGlass.Produtos.Application.CommandHandlers;
using TesteAutoGlass.Produtos.Application.Commands;
using TesteAutoGlass.Produtos.Application.Mapper;
using TesteAutoGlass.Produtos.Domain.Entities;
using Xunit;

namespace TesteAutoGlass.Produtos.Application.Tests.CommandHandlers
{
    public class ProdutoCommandHandlerTests
    {
        private readonly ProdutoCommandHandler _commandHandler;

        private readonly Mock<IProdutoRepository> _produtoRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly IMapper _autoMapper;

        public ProdutoCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            _autoMapper = config.CreateMapper();

            _unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.Setup(x => x.Commit()).ReturnsAsync(true);

            _produtoRepository = new Mock<IProdutoRepository>();
            _produtoRepository.Setup(x => x.UnitOfWork).Returns(_unitOfWork.Object);

            _commandHandler = new ProdutoCommandHandler(
                _produtoRepository.Object,
                _autoMapper);
        }

        [Fact]
        public async void DeveCriarProduto_Com_Sucesso()
        {
            var command = new CriarProdutoCommand
            {
                Fabricacao = DateTime.Now,
                FornecedorId = 1,
                Nome = "Produto teste",
                Validade = DateTime.Now.AddDays(2)
            };

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _produtoRepository.Verify(x => x.InsertAsync(It.Is<Produto>(x => x.Nome == command.Nome
                                                                            && x.Validade == command.Validade
                                                                            && x.Fabricacao == command.Fabricacao
                                                                            && x.FornecedorId == command.FornecedorId)), Times.Once);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async void CriarProduto_Deve_Retornar_Erro_FabricacaoInvalida_Igual_Validade()
        {
            var dataAtual = DateTime.Now;

            var command = new CriarProdutoCommand
            {
                Fabricacao = dataAtual,
                FornecedorId = 1,
                Nome = "Produto teste",
                Validade = dataAtual
            };

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _produtoRepository.Verify(x => x.InsertAsync(It.IsAny<Produto>()), Times.Never);
            result.IsValid.Should().BeFalse();
            result.Errors[0].ErrorMessage.Should().Be("Data de fabricação não pode ser igual ou maior a data de validade");
        }

        [Fact]
        public async void CriarProduto_Deve_Retornar_Erro_FabricacaoInvalida_Menor_Que_Validade()
        {
            var command = new CriarProdutoCommand
            {
                Fabricacao = DateTime.Now.AddDays(1),
                FornecedorId = 1,
                Nome = "Produto teste",
                Validade = DateTime.Now
            };

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _produtoRepository.Verify(x => x.InsertAsync(It.IsAny<Produto>()), Times.Never);
            result.IsValid.Should().BeFalse();
            result.Errors[0].ErrorMessage.Should().Be("Data de fabricação não pode ser igual ou maior a data de validade");
        }

        [Fact]
        public async void DeveEditarProduto_Com_Sucesso()
        {
            var command = new EditarProdutoCommand
            {
                Id = 1,
                Fabricacao = DateTime.Now,
                FornecedorId = 1,
                Nome = "Produto teste",
                Validade = DateTime.Now.AddDays(2)
            };

            var produto = new Produto
            {
                Id = 1,
            };

            _produtoRepository.Setup(x => x.GetByIdAsync(It.Is<int>(id => id == command.Id))).ReturnsAsync(produto);

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _produtoRepository.Verify(x => x.UpdateAsync(It.Is<Produto>(x => x.Id == command.Id
                                                                             && x.Nome == command.Nome
                                                                             && x.Validade == command.Validade
                                                                             && x.Fabricacao == command.Fabricacao
                                                                             && x.FornecedorId == command.FornecedorId)), Times.Once);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async void EditarProduto_Deve_Retornar_Com_Erro_Produto_NaoEncontrado()
        {
            var command = new EditarProdutoCommand
            {
                Id = 1,
                Fabricacao = DateTime.Now,
                FornecedorId = 1,
                Nome = "Produto teste",
                Validade = DateTime.Now.AddDays(2)
            };

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _produtoRepository.Verify(x => x.UpdateAsync(It.IsAny<Produto>()), Times.Never);
            result.IsValid.Should().BeFalse();
            result.Errors[0].ErrorMessage.Should().Be("Produto não encontrado");
        }

        [Fact]
        public async void EditarProduto_Deve_Retornar_Com_Erro_Fabricacao_Invalida()
        {
            var command = new EditarProdutoCommand
            {
                Id = 1,
                Fabricacao = DateTime.Now.AddDays(1),
                FornecedorId = 1,
                Nome = "Produto teste",
                Validade = DateTime.Now
            };

            var produto = new Produto
            {
                Id = 1,
            };

            _produtoRepository.Setup(x => x.GetByIdAsync(It.Is<int>(id => id == command.Id))).ReturnsAsync(produto);

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _produtoRepository.Verify(x => x.UpdateAsync(It.IsAny<Produto>()), Times.Never);
            result.IsValid.Should().BeFalse();
            result.Errors[0].ErrorMessage.Should().Be("Data de fabricação não pode ser igual ou maior a data de validade");
        }

        [Fact]
        public async void DeveExcluirProduto_Com_Sucesso()
        {
            var command = new ExcluirProdutoCommand
            {
                Id = 1
            };

            var produto = new Produto
            {
                Id = 1
            };

            _produtoRepository.Setup(x => x.GetByIdAsync(It.Is<int>(id => id == command.Id))).ReturnsAsync(produto);

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _produtoRepository.Verify(x => x.UpdateAsync(It.Is<Produto>(x => x.Id == command.Id
                                                                             && !x.Ativo)), Times.Once);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async void ExcluirProduto_Deve_Retornar_Erro_Produto_NaoEncontrado()
        {
            var command = new ExcluirProdutoCommand
            {
                Id = 1
            };

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _produtoRepository.Verify(x => x.UpdateAsync(It.IsAny<Produto>()), Times.Never);
            result.IsValid.Should().BeFalse();
            result.Errors[0].ErrorMessage.Should().Be("Produto não encontrado");
        }
    }
}
