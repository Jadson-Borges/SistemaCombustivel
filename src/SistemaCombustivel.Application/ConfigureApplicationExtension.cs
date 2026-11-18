using Microsoft.Extensions.DependencyInjection;
using SistemaCombustivel.Application.Identity;
using SistemaCombustivel.Application.Usuarios.Commands.CriarAdministradorInicial;
using SistemaCombustivel.Application.Usuarios.Commands.CriarUsuario;
using SistemaCombustivel.Application.Usuarios.Commands.DesativarUsuario;
using SistemaCombustivel.Application.Usuarios.Commands.LoginUsuario;
using SistemaCombustivel.Application.Usuarios.Commands.LogoutUsuario;
using SistemaCombustivel.Application.Usuarios.Commands.ReativarUsuario;
using SistemaCombustivel.Application.Usuarios.Queries.ListarUsuarios;
using SistemaCombustivel.Application.Veiculos.Commands.CadastrarVeiculo;
using SistemaCombustivel.Application.Veiculos.Queries.ListarVeiculos;


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
                serviceCollection.AddTransient<DesativarUsuarioCommandHandler>();
                serviceCollection.AddTransient<ReativarUsuarioCommandHandler>();
                serviceCollection.AddTransient<CadastrarVeiculoCommandHandler>();
                serviceCollection.AddTransient<ListarVeiculosQueryHandler>();

                return serviceCollection;
            }
        }
    }
}