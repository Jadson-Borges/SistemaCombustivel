using Microsoft.Extensions.DependencyInjection;
using SistemaCombustivel.Application.Identity;
using SistemaCombustivel.Application.Usuarios.Commands.CriarAdministradorInicial;
using SistemaCombustivel.Application.Usuarios.Commands.CriarUsuario;
using SistemaCombustivel.Application.Usuarios.Commands.LoginUsuario;
using SistemaCombustivel.Application.Usuarios.Commands.LogoutUsuario;
using SistemaCombustivel.Application.Usuarios.Queries.ListarUsuarios;

namespace SistemaCombustivel.Application
{
    public static class ConfigureApplicationExtension
    {
        extension(IServiceCollection serviceCollection)
        {
            public IServiceCollection ConfigurarApplication()
            {
                serviceCollection.AddTransient<BuscarUserQueryHandler>();
                serviceCollection.AddTransient<CriarUsuarioCommandHandler>();
                serviceCollection.AddTransient<CriarAdministradorInicialCommandHandler>();
                serviceCollection.AddTransient<LoginUsuarioCommandHandler>();
                serviceCollection.AddTransient<LogoutUsuarioCommandHandler>();
                serviceCollection.AddTransient<ListarUsuariosQueryHandler>();

                return serviceCollection;
            }
        }
    }
}