using SistemaCombustivel.Application.Services;

namespace SistemaCombustivel.Application.Tests.Fakes;

public sealed class CenarioDeRegistro
{
    public FakeUsuarioRepository Repositorio { get; } = new();
    public FakeServicoAutenticacao Autenticacao { get; } = new();
    public FakeUnitOfWork UnitOfWork { get; } = new();

    public RegistroUsuarioService Registro { get; }
    public AdministradorInicialService AdministradorInicial { get; }

    public CenarioDeRegistro()
    {
        // Serviços REAIS, recebendo dependências FALSAS.
        Registro = new RegistroUsuarioService(Repositorio, Autenticacao, UnitOfWork);
        AdministradorInicial = new AdministradorInicialService(Repositorio, Registro);
    }
}