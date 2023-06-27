using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TesteAutoGlass.Fornecedores.Domain.Entities;
using TesteAutoGlass.Infraestruture.Data.Repository.Produtos.Abstraction;
using TesteAutoGlass.Produtos.Application.Dtos.Filters;
using TesteAutoGlass.Produtos.Application.Mapper;
using TesteAutoGlass.Produtos.Application.Queries;
using TesteAutoGlass.Produtos.Domain.Entities;
using TesteAutoGlass.Utils.Abstractions.Pagination.Options;
using TesteAutoGlass.Utils.Abstractions.Pagination.Results;
using Xunit;

namespace TesteAutoGlass.Produtos.Application.Tests.Queries
{
    public class ProdutoQueriesTests
    {
        private readonly ProdutoQueries _produtoQueries;
        private readonly Mock<IProdutoRepository> _produtoRepository;
        private readonly IMapper _autoMapper;
        
        public ProdutoQueriesTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            _autoMapper = config.CreateMapper();

            _produtoRepository = new Mock<IProdutoRepository>();
            _produtoQueries = new ProdutoQueries(
                _produtoRepository.Object,
                _autoMapper);
        }

        [Fact]
        public async void DeveObterPorCodigo_Com_Sucesso()
        {
            int codigo = 1;

            var produto = new Produto
            {
                Codigo = codigo,
                Id = 1,
                Nome = "Produto teste"
            };

            _produtoRepository.Setup(x => x.ObterPorCodigoAsync(It.Is<int>(cod => cod == codigo)))
                .ReturnsAsync(produto);

            var result = await _produtoQueries.ObterPorCodigoAsync(codigo);

            result.Codigo.Should().Be(codigo);
            result.Id.Should().Be(produto.Id);
            result.Nome.Should().Be(produto.Nome);
        }

        [Fact]
        public async void DeveObterTodosPaginados_Com_Sucesso()
        {
            var produto = new Produto
            {
                Ativo = true,
                Id = 1,
                Codigo = 1,
                Nome = "Produto teste",
                Fornecedor = new Fornecedor
                {
                    Codigo = 1,
                    Nome = "Fornecedor teste"
                },
                Fabricacao = DateTime.Now,
                Validade = DateTime.Now.AddDays(1)
            };

            var pageOptions = new PageOptionsProdutoDto
            {
                Filtro = new ProdutoFiltroDto
                {
                    Nome = "Produto teste"
                },
                Page = 1,
                Size = 10,
                SortOptions = new SortOptionsDto
                {
                    Ascending = true,
                    SortColumns = new string[]
                    {
                        "Nome"
                    }
                },
                TotalPages = 1
            };

            _produtoRepository.Setup(x => x.Query())
                .Returns(new List<Produto> { produto }.AsQueryable());

            _produtoRepository.Setup(x => x.GetPaged(It.IsAny<IQueryable<Produto>>(), It.Is<PageOptionsProdutoDto>(x => x.Filtro.Nome == pageOptions.Filtro.Nome)))
                .ReturnsAsync(new PagedResult<Produto>
                {
                    CurrentPage = 1,
                    PageCount = pageOptions.TotalPages,
                    PageSize = pageOptions.Size,
                    Results = new List<Produto> { produto }
                });

            var result = await _produtoQueries.ObterTodosPaginadoAsync(pageOptions);

            result.CurrentPage.Should().Be(1);
            result.PageCount.Should().Be(pageOptions.TotalPages);
            result.PageSize.Should().Be(pageOptions.Size);
            result.Results.FirstOrDefault().Id.Should().Be(produto.Id);
            result.Results.FirstOrDefault().Codigo.Should().Be(produto.Codigo);
            result.Results.FirstOrDefault().Nome.Should().Be(produto.Nome);
            result.Results.FirstOrDefault().Fornecedor.Should().Be("Fornecedor teste");
        }
    }
}
