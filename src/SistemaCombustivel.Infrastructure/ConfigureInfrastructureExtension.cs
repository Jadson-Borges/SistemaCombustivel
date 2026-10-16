using Microsoft.Extensions.DependencyInjection;
using SistemaCombustivel.Application.Identity;
using SistemaCombustivel.Application.Usuarios.Commands.CriarAdministradorInicial;
using SistemaCombustivel.Application.Usuarios.Commands.CriarUsuario;
using SistemaCombustivel.Application.Usuarios.Commands.RegistrarUsuario;
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
                ServiceCollection.AddTransient<CriarUsuarioCommandHandler>();
                ServiceCollection.AddTransient<RegistrarUsuarioCommandHandler>();
                ServiceCollection.AddTransient<CriarAdministradorInicialCommandHandler>();

                return ServiceCollection;
            }
        }

        //public static IServiceCollection ConfigurarInfra(this IServiceCollection ServiceCollection)
        //{


        //    return ServiceCollection;
        //}
    }
}
