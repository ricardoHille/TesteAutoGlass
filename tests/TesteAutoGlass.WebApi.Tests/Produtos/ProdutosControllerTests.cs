using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TesteAutoGlass.Api.Controllers.Produtos;
using TesteAutoGlass.Produtos.Application.Commands;
using TesteAutoGlass.Produtos.Application.Dtos.Filters;
using TesteAutoGlass.Produtos.Application.Dtos.Requests;
using TesteAutoGlass.Produtos.Application.Dtos.Responses;
using TesteAutoGlass.Produtos.Application.Mapper;
using TesteAutoGlass.Produtos.Application.Queries.Abstraction;
using TesteAutoGlass.Utils.Abstractions.Command.MediatorHandler.Abstraction;
using TesteAutoGlass.Utils.Abstractions.Pagination.Results;
using Xunit;

namespace TesteAutoGlass.WebApi.Tests.Produtos
{
    public class ProdutosControllerTests
    {
        private readonly Mock<IProdutoQueries> _queries;
        private readonly Mock<IMediatorHandler> _mediator;
        private readonly IMapper _autoMapper;
        private readonly ProdutosController _controller;

        public ProdutosControllerTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            _autoMapper = config.CreateMapper();

            _queries = new Mock<IProdutoQueries>();
            _mediator = new Mock<IMediatorHandler>();

            _controller = new ProdutosController(
                _queries.Object,
                _autoMapper,
                _mediator.Object);
        }

        [Fact]
        public async void DeveObterTodosPaginados_Com_Sucesso()
        {
            var produtoListagem = new ProdutoListagemDto
            {
                Codigo = 1,
                Id = 1,
                Nome = "teste"
            };

            var pageOptions = new PageOptionsProdutoDto
            {
                Filtro = new ProdutoFiltroDto
                {
                    Nome = "teste"
                },
                Page = 1,
                Size = 10,
                TotalPages = 1
            };

            var pagedResult = new PagedResult<ProdutoListagemDto>
            {
                CurrentPage = 1,
                PageCount = 1,
                PageSize = 10,
                Results = new List<ProdutoListagemDto> { produtoListagem }
            };

            _queries.Setup(x => x.ObterTodosPaginadoAsync(pageOptions))
                .ReturnsAsync(pagedResult);

            var result = await _controller.ObterTodos(pageOptions);

            result.PageSize.Should().Be(10);
            result.PageCount.Should().Be(1);
            result.CurrentPage.Should().Be(1);
            result.Results.FirstOrDefault().Nome.Should().Be(produtoListagem.Nome);
            result.Results.FirstOrDefault().Id.Should().Be(produtoListagem.Id);
            result.Results.FirstOrDefault().Codigo.Should().Be(produtoListagem.Codigo);
        }

        [Fact]
        public async void DeveObterPorCodigo_Com_Sucesso()
        {
            int codigo = 1;

            var produtoExibicao = new ProdutoExibicaoDto
            {
                Codigo = codigo,
                Id = 1,
                Nome = "teste"
            };

            _queries.Setup(x => x.ObterPorCodigoAsync(It.Is<int>(c => c == codigo)))
                .ReturnsAsync(produtoExibicao);

            var result = await _controller.ObterPorCodigo(codigo);

            result.Nome.Should().Be(produtoExibicao.Nome);
            result.Codigo.Should().Be(codigo);
            result.Id.Should().Be(produtoExibicao.Id);
        }

        [Fact]
        public async void DeveCadastrarProduto_Com_Sucesso()
        {
            var dto = new ProdutoCriacaoDto
            {
                Fabricacao = DateTime.Now,
                FornecedorId = 1,
                Nome = "produto teste",
                Validade = DateTime.Now.AddDays(1)
            };

            var result = await _controller.CadastrarProduto(dto);

            _mediator.Verify(x => x.SendCommandAsync(It.Is<CriarProdutoCommand>(x => x.Nome.Equals(dto.Nome)
                                                                                     && x.FornecedorId == dto.FornecedorId
                                                                                     && x.Fabricacao == dto.Fabricacao 
                                                                                     && x.Validade == dto.Validade)), Times.Once);
        }

        [Fact]
        public async void DeveEditarProduto_Com_Sucesso()
        {
            var id = 1;
            var dto = new ProdutoEdicaoDto
            {
                Id = id,
                Fabricacao = DateTime.Now,
                FornecedorId = 1,
                Nome = "produto teste",
                Validade = DateTime.Now.AddDays(1)
            };

            var result = await _controller.EditarProduto(dto);

            _mediator.Verify(x => x.SendCommandAsync(It.Is<EditarProdutoCommand>(x => x.Id == dto.Id
                                                                                      && x.Nome.Equals(dto.Nome)
                                                                                      && x.FornecedorId == dto.FornecedorId
                                                                                      && x.Fabricacao == dto.Fabricacao
                                                                                      && x.Validade == dto.Validade)), Times.Once);
        }

        [Fact]
        public async void DeveExcluirProduto_Com_Sucesso()
        {
            var id = 1;

            var dto = new ExcluirProdutoCommand
            {
                Id = id
            };

            var result = await _controller.ExcluirProduto(id);

            _mediator.Verify(x => x.SendCommandAsync(It.Is<ExcluirProdutoCommand>(x => x.Id == dto.Id)), Times.Once);
        }
    }
}
