using SistemaCombustivel.Application.Usuarios.Commands.CriarAdministradorInicial;
using SistemaCombustivel.Application.Usuarios.Commands.CriarUsuario;
using SistemaCombustivel.Application.Usuarios.Commands.RegistrarUsuario;

namespace SistemaCombustivel.Application.Tests.Fakes;

public sealed class CenarioDeRegistro
{
    public FakeUsuarioRepository Repositorio { get; } = new();
    public FakeServicoAutenticacao Autenticacao { get; } = new();
    public FakeUnitOfWork UnitOfWork { get; } = new();

    public CriarUsuarioCommandHandler CriarUsuario { get; }
    public RegistrarUsuarioCommandHandler Registrar { get; }
    public CriarAdministradorInicialCommandHandler AdministradorInicial { get; }

    public CenarioDeRegistro()
    {
        CriarUsuario = new CriarUsuarioCommandHandler(Repositorio, Autenticacao, UnitOfWork);
        Registrar = new RegistrarUsuarioCommandHandler(CriarUsuario);
        AdministradorInicial = new CriarAdministradorInicialCommandHandler(Repositorio, CriarUsuario);
    }
}