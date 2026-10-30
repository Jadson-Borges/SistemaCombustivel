using Microsoft.Extensions.DependencyInjection;
using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Application.Identity;
using SistemaCombustivel.Infrastructure.Data;
using SistemaCombustivel.Infrastructure.Identity;
using SistemaCombustivel.Infrastructure.Repositories;

namespace SistemaCombustivel.Infrastructure
{
    public static class ConfigureInfrastructureExtension
    {
        extension(IServiceCollection ServiceCollection)
        {
            public IServiceCollection ConfigurarInfra()
            {
                ServiceCollection.AddScoped<IUsuarioRepository, UsuarioRepository>();
                ServiceCollection.AddScoped<IServicoAutenticacao, ServicoAutenticacao>();
                ServiceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
                ServiceCollection.AddScoped<IUserRepository, UserRepository>();

                return ServiceCollection;
            }
        }

        //public static IServiceCollection ConfigurarInfra(this IServiceCollection ServiceCollection)
        //{


        //    return ServiceCollection;
        //}
    }
}
