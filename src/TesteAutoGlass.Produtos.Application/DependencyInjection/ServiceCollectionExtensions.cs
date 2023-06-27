using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TesteAutoGlass.Infraestruture.Data.DependencyInjection;
using TesteAutoGlass.Produtos.Application.CommandHandlers;
using TesteAutoGlass.Produtos.Application.Commands;
using TesteAutoGlass.Produtos.Application.Mapper;
using TesteAutoGlass.Produtos.Application.Queries;
using TesteAutoGlass.Produtos.Application.Queries.Abstraction;

namespace TesteAutoGlass.Produtos.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddProdutoApplicationServices(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(MapperProfile));

            // Infrastructure
            services.AddInfrastructureServices();

            // Services

            services.AddScoped<IRequestHandler<CriarProdutoCommand, ValidationResult>, ProdutoCommandHandler>();
            services.AddScoped<IRequestHandler<EditarProdutoCommand, ValidationResult>, ProdutoCommandHandler>();
            services.AddScoped<IRequestHandler<ExcluirProdutoCommand, ValidationResult>, ProdutoCommandHandler>();

            services.AddScoped<IProdutoQueries, ProdutoQueries>();
        }
    }
}
