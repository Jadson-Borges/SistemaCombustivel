using SistemaCombustivel.Application.Usuarios.Commands.CriarAdministradorInicial;
using SistemaCombustivel.Application.Usuarios.Commands.CriarUsuario;

namespace SistemaCombustivel.Application.Tests.Fakes;

public sealed class CenarioDeRegistro
{
    public FakeUsuarioRepository Repositorio { get; } = new();
    public FakeServicoAutenticacao Autenticacao { get; } = new();
    public FakeUnitOfWork UnitOfWork { get; } = new();

    public CriarUsuarioCommandHandler CriarUsuario { get; }
    public CriarAdministradorInicialCommandHandler AdministradorInicial { get; }

    public CenarioDeRegistro()
    {
        CriarUsuario = new CriarUsuarioCommandHandler(Repositorio, Autenticacao, UnitOfWork);
        AdministradorInicial = new CriarAdministradorInicialCommandHandler(Repositorio, CriarUsuario);
    }
}