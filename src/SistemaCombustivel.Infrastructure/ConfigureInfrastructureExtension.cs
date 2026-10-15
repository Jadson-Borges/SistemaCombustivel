using Microsoft.Extensions.DependencyInjection;
using SistemaCombustivel.Application.Identity;
using SistemaCombustivel.Infrastructure.Identity;

namespace SistemaCombustivel.Infrastructure
{
    public static class ConfigureInfrastructureExtension
    {
        extension(IServiceCollection ServiceCollection)
        {
            public IServiceCollection ConfigurarInfra()
            {
                ServiceCollection.AddScoped<IUserRepository, UserRepository>();
                ServiceCollection.AddTransient<BuscarUserQueryHandler>();

                return ServiceCollection;
            }
        }

        //public static IServiceCollection ConfigurarInfra(this IServiceCollection ServiceCollection)
        //{


        //    return ServiceCollection;
        //}
    }
}
