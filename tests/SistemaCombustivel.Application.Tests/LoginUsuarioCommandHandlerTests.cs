using SistemaCombustivel.Application.Tests.Fakes;
using SistemaCombustivel.Application.Usuarios.Commands.LoginUsuario;

namespace SistemaCombustivel.Application.Tests;

public class LoginUsuarioCommandHandlerTests
{
    private readonly FakeUsuarioRepository _repositorio = new();
    private readonly FakeServicoAutenticacao _autenticacao = new();

    private LoginUsuarioCommandHandler CriarHandler() => new(_autenticacao, _repositorio);

    [Fact]
    public async Task Handle_CredenciaisValidasEUsuarioAtivo_IniciaASessao()
    {
        _repositorio.Usuarios.Add(UsuarioDeTeste.Criar(1, "Maria"));
        _autenticacao.UsuarioIdDoLogin = 1;

        var resultado = await CriarHandler().Handle(
            new LoginUsuarioCommand("maria@teste.com", "Senha123"), CancellationToken.None);

        Assert.True(resultado.Sucesso);
        Assert.Equal("maria@teste.com", Assert.Single(_autenticacao.Entradas));
    }

    [Fact]
    public async Task Handle_CredenciaisInvalidas_FalhaSemIniciarSessao()
    {
        _autenticacao.FalharNoLogin = true;
        _autenticacao.MensagemDeErroLogin = "Credenciais inválidas.";

        var resultado = await CriarHandler().Handle(
            new LoginUsuarioCommand("maria@teste.com", "errada"), CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Equal("Credenciais inválidas.", resultado.Erro);
        Assert.Empty(_autenticacao.Entradas);
    }

    [Fact]
    public async Task Handle_SenhaCertaMasUsuarioInativo_RecusaComMensagemDeInativo()
    {
        _repositorio.Usuarios.Add(UsuarioDeTeste.Criar(1, "Maria", ativo: false));
        _autenticacao.UsuarioIdDoLogin = 1;

        var resultado = await CriarHandler().Handle(
            new LoginUsuarioCommand("maria@teste.com", "Senha123"), CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Equal(LoginUsuarioCommandHandler.MensagemUsuarioInativo, resultado.Erro);
        Assert.Empty(_autenticacao.Entradas);
    }
}