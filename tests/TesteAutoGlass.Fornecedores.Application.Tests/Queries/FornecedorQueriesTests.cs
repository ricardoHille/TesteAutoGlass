using AutoMapper;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using TesteAutoGlass.Fornecedores.Application.Mapper;
using TesteAutoGlass.Fornecedores.Application.Queries;
using TesteAutoGlass.Fornecedores.Domain.Entities;
using TesteAutoGlass.Infraestruture.Data.Repository.Fornecedores.Abstraction;
using Xunit;

namespace TesteAutoGlass.Fornecedores.Application.Tests.Queries
{
    public class FornecedorQueriesTests
    {
        private readonly Mock<IFornecedorRepository> _fornecedorRepository;
        private readonly IMapper _autoMapper;
        private readonly FornecedorQueries _fornecedorQueries;

        public FornecedorQueriesTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            _autoMapper = config.CreateMapper();

            _fornecedorRepository = new Mock<IFornecedorRepository>();

            _fornecedorQueries = new FornecedorQueries(
                _fornecedorRepository.Object,
                _autoMapper);
        }

        [Fact]
        public async void DeveObterPorId_Com_Sucesso()
        {
            int id = 1;
            var fornecedor = new Fornecedor
            {
                Id = id,
                Cnpj = "11111111111111",
                Nome = "Fornecedor teste",
                Codigo = 1,
            };

            _fornecedorRepository.Setup(x => x.GetByIdAsync(It.Is<int>(i => i == id)))
                .ReturnsAsync(fornecedor);

            var result = await _fornecedorQueries.ObterPorIdAsync(id);

            result.Nome.Should().Be(fornecedor.Nome);
            result.Cnpj.Should().Be(fornecedor.Cnpj);
            result.Codigo.Should().Be(fornecedor.Codigo);
            result.Id.Should().Be(fornecedor.Id);
        }

        [Fact]
        public async void DeveObterTodos_Com_Sucesso()
        {
            var fornecedores = new List<Fornecedor>
            {
                new Fornecedor
                {
                    Id = 1,
                    Cnpj = "11111111111111",
                    Nome = "Fornecedor teste",
                    Codigo = 1,
                    Ativo = true,
                },
                new Fornecedor
                {
                    Id = 2,
                    Cnpj = "22222222222222",
                    Nome = "Fornecedor teste 2",
                    Codigo = 2,
                    Ativo = false,
                }
            };

            _fornecedorRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(fornecedores);

            var result = await _fornecedorQueries.ObterTodosAsync();

            result.Count().Should().Be(1);
            result.FirstOrDefault().Nome.Should().Be("Fornecedor teste");
            result.FirstOrDefault().Cnpj.Should().Be("11111111111111");
            result.FirstOrDefault().Codigo.Should().Be(1);
            result.FirstOrDefault().Id.Should().Be(1);
        }
    }
}
