using SistemaCombustivel.Application.Tests.Fakes;
using SistemaCombustivel.Application.Usuarios.Commands.LoginUsuario;

namespace SistemaCombustivel.Application.Tests;

public class LoginUsuarioCommandHandlerTests
{
    [Fact]
    public async Task Handle_ComCredenciaisValidas_DevolveSucessoEEncaminhaAsCredenciais()
    {
        var autenticacao = new FakeServicoAutenticacao();
        var handler = new LoginUsuarioCommandHandler(autenticacao);

        var resultado = await handler.Handle(
            new LoginUsuarioCommand("maria@teste.com", "Senha123"), CancellationToken.None);

        Assert.True(resultado.Sucesso);
        var login = Assert.Single(autenticacao.Logins);
        Assert.Equal("maria@teste.com", login.Email);
        Assert.Equal("Senha123", login.Senha);
    }

    [Fact]
    public async Task Handle_ComCredenciaisInvalidas_DevolveFalhaComAMensagemDaAutenticacao()
    {
        var autenticacao = new FakeServicoAutenticacao
        {
            FalharNoLogin = true,
            MensagemDeErroLogin = "Credenciais inválidas."
        };
        var handler = new LoginUsuarioCommandHandler(autenticacao);

        var resultado = await handler.Handle(
            new LoginUsuarioCommand("maria@teste.com", "errada"), CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Equal("Credenciais inválidas.", resultado.Erro);
    }
}