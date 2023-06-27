using AutoMapper;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using TesteAutoGlass.Api.Controllers.Fornecedores;
using TesteAutoGlass.Fornecedores.Application.Commands;
using TesteAutoGlass.Fornecedores.Application.Dtos;
using TesteAutoGlass.Fornecedores.Application.Dtos.Requests;
using TesteAutoGlass.Fornecedores.Application.Dtos.Responses;
using TesteAutoGlass.Fornecedores.Application.Mapper;
using TesteAutoGlass.Fornecedores.Application.Queries.Abstraction;
using TesteAutoGlass.Utils.Abstractions.Command.MediatorHandler.Abstraction;
using Xunit;

namespace TesteAutoGlass.WebApi.Tests.Fornecedores
{
    public class FornecedoresControllerTests
    {
        private readonly Mock<IFornecedorQueries> _queries;
        private readonly Mock<IMediatorHandler> _mediator;
        private readonly IMapper _autoMapper;
        private readonly FornecedoresController _controller;

        public FornecedoresControllerTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            _autoMapper = config.CreateMapper();

            _queries = new Mock<IFornecedorQueries>();
            _mediator = new Mock<IMediatorHandler>();

            _controller = new FornecedoresController(
                _queries.Object,
                _autoMapper,
                _mediator.Object);
        }

        [Fact]
        public async void DeveObterTodos_Com_Sucesso()
        {
            var fornecedoresExibicao = new List<FornecedorListagemDto>()
            {
                new FornecedorListagemDto
                {
                    Cnpj = "23405692000153",
                    Codigo = 1,
                    Id = 1,
                    Nome = "teste"
                }
            };

            _queries.Setup(x => x.ObterTodosAsync())
                .ReturnsAsync(fornecedoresExibicao);

            var result = await _controller.ObterTodos();

            result.FirstOrDefault().Nome.Should().Be("teste");
            result.FirstOrDefault().Cnpj.Should().Be("23405692000153");
            result.FirstOrDefault().Id.Should().Be(1);
            result.FirstOrDefault().Codigo.Should().Be(1);
        }

        [Fact]
        public async void DeveObterPorId_Com_Sucesso()
        {
            int id = 1;

            var fornecedorExibicao = new FornecedorExibicaoDto
            {
                Cnpj = "23405692000153",
                Codigo = 1,
                Id = id,
                Nome = "teste"
            };

            _queries.Setup(x => x.ObterPorIdAsync(It.Is<int>(i => i == id)))
                .ReturnsAsync(fornecedorExibicao);

            var result = await _controller.ObterPorId(id);

            result.Nome.Should().Be(fornecedorExibicao.Nome);
            result.Codigo.Should().Be(fornecedorExibicao.Codigo);
            result.Id.Should().Be(id);
            result.Cnpj.Should().Be(fornecedorExibicao.Cnpj);
        }

        [Fact]
        public async void DeveCadastrarFornecedor_Com_Sucesso()
        {
            var dto = new FornecedorCriacaoDto
            {
                Cnpj = "23405692000153",
                Nome = "Fornecedor teste"
            };

            var result = await _controller.CadastrarFornecedor(dto);

            _mediator.Verify(x => x.SendCommandAsync(It.Is<CriarFornecedorCommand>(x => x.Cnpj.Equals(dto.Cnpj) 
                                                                                        && x.Nome.Equals(dto.Nome))), Times.Once);
        }

        [Fact]
        public async void DeveEditarFornecedor_Com_Sucesso()
        {
            var dto = new FornecedorEdicaoDto
            {
                Id = 1,
                Cnpj = "23405692000153",
                Nome = "Fornecedor teste"
            };

            var result = await _controller.EditarFornecedor(dto);

            _mediator.Verify(x => x.SendCommandAsync(It.Is<EditarFornecedorCommand>(x => x.Cnpj.Equals(dto.Cnpj) 
                                                                                        && x.Nome.Equals(dto.Nome)
                                                                                        && x.Id == dto.Id)), Times.Once);
        }

        [Fact]
        public async void DeveExcluirFornecedor_Com_Sucesso()
        {
            var id = 1;

            var dto = new ExcluirFornecedorCommand
            {
                Id = id
            };

            var result = await _controller.ExcluirFornecedor(id);

            _mediator.Verify(x => x.SendCommandAsync(It.Is<ExcluirFornecedorCommand>(x => x.Id == dto.Id)), Times.Once);
        }
    }
}
