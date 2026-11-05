using SistemaCombustivel.Application.Tests.Fakes;
using SistemaCombustivel.Application.Usuarios.Commands.DesativarUsuario;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Tests;

public class DesativarUsuarioCommandHandlerTests
{
    private readonly FakeUsuarioRepository _repositorio = new();
    private readonly FakeServicoAutenticacao _autenticacao = new();

    private DesativarUsuarioCommandHandler CriarHandler() => new(_repositorio, _autenticacao);

    [Fact]
    public async Task Handle_UsuarioComum_DesativaEInvalidaSessoes()
    {
        var maria = UsuarioDeTeste.Criar(2, "Maria");
        _repositorio.Usuarios.Add(maria);

        var resultado = await CriarHandler().Handle(new DesativarUsuarioCommand(2), CancellationToken.None);

        Assert.True(resultado.Sucesso);
        Assert.False(maria.Ativo);
        Assert.Equal(new[] { 2 }, _autenticacao.SessoesInvalidadas);
    }

    [Fact]
    public async Task Handle_UnicoAdministradorAtivo_RecusaEMantemAtivo()
    {
        var admin = UsuarioDeTeste.Criar(1, "Admin", TipoUsuario.Administrador);
        _repositorio.Usuarios.Add(admin);

        var resultado = await CriarHandler().Handle(new DesativarUsuarioCommand(1), CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.True(admin.Ativo);
        Assert.Empty(_autenticacao.SessoesInvalidadas);
    }

    [Fact]
    public async Task Handle_AdministradorComOutroAdminAtivo_Desativa()
    {
        var admin1 = UsuarioDeTeste.Criar(1, "Admin", TipoUsuario.Administrador);
        var admin2 = UsuarioDeTeste.Criar(2, "Ana", TipoUsuario.Administrador);
        _repositorio.Usuarios.AddRange(new[] { admin1, admin2 });

        var resultado = await CriarHandler().Handle(new DesativarUsuarioCommand(2), CancellationToken.None);

        Assert.True(resultado.Sucesso);
        Assert.False(admin2.Ativo);
        Assert.True(admin1.Ativo);
    }

    [Fact]
    public async Task Handle_UsuarioInexistente_Falha()
    {
        var resultado = await CriarHandler().Handle(new DesativarUsuarioCommand(99), CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Equal("Usuário não encontrado.", resultado.Erro);
    }

    [Fact]
    public async Task Handle_UsuarioJaInativo_Falha()
    {
        _repositorio.Usuarios.Add(UsuarioDeTeste.Criar(2, "Maria", ativo: false));

        var resultado = await CriarHandler().Handle(new DesativarUsuarioCommand(2), CancellationToken.None);

        Assert.False(resultado.Sucesso);
    }
}