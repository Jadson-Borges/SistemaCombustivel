using SistemaCombustivel.Application.Tests.Fakes;
using SistemaCombustivel.Application.Usuarios.Commands.LogoutUsuario;

namespace SistemaCombustivel.Application.Tests;

public class LogoutUsuarioCommandHandlerTests
{
    [Fact]
    public async Task Handle_EncerraASessaoUmaVez()
    {
        var autenticacao = new FakeServicoAutenticacao();
        var handler = new LogoutUsuarioCommandHandler(autenticacao);

        await handler.Handle(new LogoutUsuarioCommand(), CancellationToken.None);

        Assert.Equal(1, autenticacao.Logouts);
    }
}