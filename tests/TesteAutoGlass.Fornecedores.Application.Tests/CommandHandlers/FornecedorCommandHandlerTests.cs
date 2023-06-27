using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using TesteAutoGlass.Fornecedores.Application.CommandHandlers;
using TesteAutoGlass.Fornecedores.Application.Commands;
using TesteAutoGlass.Fornecedores.Application.Mapper;
using TesteAutoGlass.Fornecedores.Domain.Entities;
using TesteAutoGlass.Infraestruture.Data.Context.Abstraction;
using TesteAutoGlass.Infraestruture.Data.Repository.Fornecedores.Abstraction;
using Xunit;

namespace TesteAutoGlass.Fornecedores.Application.Tests.CommandHandlers
{
    public class FornecedorCommandHandlerTests
    {
        private readonly FornecedorCommandHandler _commandHandler;
        private readonly IMapper _autoMapper;
        private readonly Mock<IFornecedorRepository> _fornecedorRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;

        public FornecedorCommandHandlerTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            _autoMapper = config.CreateMapper();

            _unitOfWork = new Mock<IUnitOfWork>();
            _fornecedorRepository = new Mock<IFornecedorRepository>();

            _unitOfWork.Setup(x => x.Commit()).ReturnsAsync(true);
            _fornecedorRepository.Setup(x => x.UnitOfWork).Returns(_unitOfWork.Object);

            _commandHandler = new FornecedorCommandHandler(
                _fornecedorRepository.Object,
                _autoMapper);
        }

        [Fact]
        public async void DeveCriarFornecedor_Com_Sucesso()
        {
            var command = new CriarFornecedorCommand
            {
                Cnpj = "23405692000153",
                Nome = "Fornecedor teste"
            };

            _fornecedorRepository.Setup(x => x.Find(It.IsAny<Expression<Func<Fornecedor, bool>>>())).Returns(new List<Fornecedor>());

            var result = await _commandHandler.Handle(command, new CancellationToken());

            result.IsValid.Should().BeTrue();

            _fornecedorRepository.Verify(x => x.InsertAsync(It.Is<Fornecedor>(x => x.Cnpj.Equals(command.Cnpj) && x.Nome.Equals(command.Nome))), Times.Once);
        }

        [Fact]
        public async void CriarFornecedor_Deve_Retornar_Com_Erro_FornecedorExistente()
        {
            var command = new CriarFornecedorCommand
            {
                Cnpj = "23405692000153",
                Nome = "Fornecedor teste"
            };

            var fornecedor = new Fornecedor
            {
                Cnpj = "23405692000153",
                Nome = "Fornecedor",
                Ativo = true
            };

            _fornecedorRepository.Setup(x => x.Find(It.Is<Expression<Func<Fornecedor, bool>>>(exp => exp.Compile()(fornecedor))))
                .Returns(new List<Fornecedor> { fornecedor});

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _fornecedorRepository.Verify(x => x.InsertAsync(It.IsAny<Fornecedor>()), Times.Never);

            result.IsValid.Should().BeFalse();
            result.Errors[0].ErrorMessage.Should().Be($"Fornecedor com cnpj {fornecedor.Cnpj} já existe na base de dados");

        }

        [Fact]
        public async void CriarFornecedor_Deve_Retornar_Com_Erro_CnpjInvalido()
        {
            var command = new CriarFornecedorCommand
            {
                Cnpj = "23405692000152",
                Nome = "Fornecedor teste",
            };

            _fornecedorRepository.Setup(x => x.Find(It.IsAny<Expression<Func<Fornecedor, bool>>>())).Returns(new List<Fornecedor>()); 

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _fornecedorRepository.Verify(x => x.InsertAsync(It.IsAny<Fornecedor>()), Times.Never);

            result.IsValid.Should().BeFalse();
            result.Errors[0].ErrorMessage.Should().Be($"Cnpj {command.Cnpj} não é valido");
        }

        [Fact]
        public async void DeveEditarFornecedor_Com_Sucesso()
        {
            var command = new EditarFornecedorCommand
            {
                Id = 1,
                Cnpj = "23405692000153",
                Nome = "Fornecedor editado",
            };

            var fornecedor = new Fornecedor
            {
                Id = 1,
                Cnpj = "23405692000153",
                Nome = "Fornecedor",
                Ativo = true
            };

            _fornecedorRepository.Setup(x => x.Find(It.Is<Expression<Func<Fornecedor, bool>>>(exp => exp.Compile()(fornecedor))))
                .Returns(new List<Fornecedor> { fornecedor });

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _fornecedorRepository.Verify(x => x.UpdateAsync(It.Is<Fornecedor>(x => x.Nome.Equals(command.Nome))), Times.Once);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async void EditarFornecedor_Deve_Retornar_Com_Erro_Fornecedor_Nao_Encontrado()
        {
            var command = new EditarFornecedorCommand
            {
                Id = 1,
                Cnpj = "23405692000153",
                Nome = "Fornecedor editado",
            };

            var fornecedor = new Fornecedor
            {
                Id = 2,
                Cnpj = "23405692000153",
                Nome = "Fornecedor"
            };

            _fornecedorRepository.Setup(x => x.Find(It.Is<Expression<Func<Fornecedor, bool>>>(exp => exp.Compile()(fornecedor))))
                .Returns(new List<Fornecedor> { fornecedor });

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _fornecedorRepository.Verify(x => x.UpdateAsync(It.Is<Fornecedor>(x => x.Nome.Equals(command.Nome))), Times.Never);

            result.IsValid.Should().BeFalse();
            result.Errors[0].ErrorMessage.Should().Be($"Fornecedor não encontrado");
        }

        [Fact]
        public async void EditarFornecedor_Deve_Retornar_Com_Erro_Cnpj_Editado_Ja_Existe_Na_Base()
        {
            var command = new EditarFornecedorCommand
            {
                Id = 1,
                Cnpj = "23405692000153",
                Nome = "Fornecedor editado",
            };

            var fornecedores = new List<Fornecedor>
            {
                new Fornecedor
                {
                    Id = 2,
                    Cnpj = "23405692000153",
                    Nome = "Fornecedor",
                    Ativo = true
                },
                new Fornecedor
                {
                    Id = 1,
                    Cnpj = "49647080000160",
                    Nome = "Fornecedor 1",
                    Ativo = true
                }
            };

            _fornecedorRepository.Setup(x => x.Find(It.Is<Expression<Func<Fornecedor, bool>>>(exp => exp.Compile()(fornecedores.First()))))
                .Returns(fornecedores);

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _fornecedorRepository.Verify(x => x.UpdateAsync(It.Is<Fornecedor>(x => x.Nome.Equals(command.Nome))), Times.Never);

            result.IsValid.Should().BeFalse();
            result.Errors[0].ErrorMessage.Should().Be($"Não foi possível editar o fornecedor, pois já existe um fornecedor com o mesmo cnpj {command.Cnpj} na base de dados");
        }

        [Fact]
        public async void EditarFornecedor_Deve_Retornar_Com_Erro_CnpjInvalido()
        {
            var command = new EditarFornecedorCommand
            {
                Id = 1,
                Cnpj = "23405692000152",
                Nome = "Fornecedor editado",
            };

            var fornecedor = new Fornecedor
            {
                Id = 1,
                Cnpj = "23405692000153",
                Nome = "Fornecedor",
                Ativo = true
            };

            _fornecedorRepository.Setup(x => x.Find(It.Is<Expression<Func<Fornecedor, bool>>>(exp => exp.Compile()(fornecedor))))
                .Returns(new List<Fornecedor> { fornecedor });

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _fornecedorRepository.Verify(x => x.UpdateAsync(It.IsAny<Fornecedor>()), Times.Never);

            result.IsValid.Should().BeFalse();
            result.Errors[0].ErrorMessage.Should().Be($"Cnpj {command.Cnpj} não é valido");
        }

        [Fact]
        public async void DeveExcluirFornecedor_Com_Sucesso()
        {
            var command = new ExcluirFornecedorCommand
            {
                Id = 1,
            };

            var fornecedor = new Fornecedor
            {
                Id = 1
            };

            _fornecedorRepository.Setup(x => x.GetByIdAsync(It.Is<int>(id => id == command.Id))).ReturnsAsync(fornecedor);

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _fornecedorRepository.Verify(x => x.UpdateAsync(It.Is<Fornecedor>(x => x.Id == command.Id
                                                                                   && !x.Ativo)), Times.Once);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async void ExcluirFornecedor_Deve_Retornar_Com_Erro_Fornecedor_NaoEncontrado()
        {
            var command = new ExcluirFornecedorCommand
            {
                Id = 1,
            };

            var result = await _commandHandler.Handle(command, new CancellationToken());

            _fornecedorRepository.Verify(x => x.UpdateAsync(It.IsAny<Fornecedor>()), Times.Never);
            result.IsValid.Should().BeFalse();
            result.Errors[0].ErrorMessage.Should().Be($"Fornecedor não encontrado");
        }
    }
}
