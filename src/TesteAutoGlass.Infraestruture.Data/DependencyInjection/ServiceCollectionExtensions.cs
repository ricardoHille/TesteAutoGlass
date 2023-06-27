using Microsoft.Extensions.DependencyInjection;
using TesteAutoGlass.Infraestruture.Data.Context;
using TesteAutoGlass.Infraestruture.Data.Repository.Fornecedores.Abstraction;
using TesteAutoGlass.Infraestruture.Data.Repository.Produtos.Abstraction;
using TesteAutoGlass.Utils.Abstraction.Infra.Data.Repository.Fornecedores;
using TesteAutoGlass.Utils.Abstraction.Infra.Data.Repository.Produtos;

namespace TesteAutoGlass.Infraestruture.Data.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            // DbContext
            services.AddDbContext<ApplicationContext>();

            // Repositories
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
        }
    }
}
