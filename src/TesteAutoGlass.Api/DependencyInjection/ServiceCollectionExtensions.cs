using Microsoft.Extensions.DependencyInjection;
using TesteAutoGlass.Utils.Abstractions.Command.MediatorHandler;
using TesteAutoGlass.Utils.Abstractions.Command.MediatorHandler.Abstraction;

namespace TesteAutoGlass.Api.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ResolveDependecies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IMediatorHandler, MediatorHandler>();

            return serviceCollection;
        }
    }
}
