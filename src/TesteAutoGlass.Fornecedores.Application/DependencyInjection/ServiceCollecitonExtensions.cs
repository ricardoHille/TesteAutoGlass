using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TesteAutoGlass.Fornecedores.Application.CommandHandlers;
using TesteAutoGlass.Fornecedores.Application.Commands;
using TesteAutoGlass.Fornecedores.Application.Mapper;
using TesteAutoGlass.Fornecedores.Application.Queries;
using TesteAutoGlass.Fornecedores.Application.Queries.Abstraction;
using TesteAutoGlass.Infraestruture.Data.DependencyInjection;

namespace TesteAutoGlass.Fornecedores.Application.DependencyInjection
{
    public static class ServiceCollecitonExtensions
    {
        public static void AddFornecedorApplicationServices(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(typeof(MapperProfile));

            // Infrastructure
            services.AddInfrastructureServices();

            // Services

            services.AddScoped<IRequestHandler<CriarFornecedorCommand, ValidationResult>, FornecedorCommandHandler>();
            services.AddScoped<IRequestHandler<EditarFornecedorCommand, ValidationResult>, FornecedorCommandHandler>();
            services.AddScoped<IRequestHandler<ExcluirFornecedorCommand, ValidationResult>, FornecedorCommandHandler>();

            services.AddScoped<IFornecedorQueries, FornecedorQueries>();
        }
    }
}
