using SistemaCombustivel.Application.Identity;
using SistemaCombustivel.Application.Tests.Fakes;

namespace SistemaCombustivel.Application.Tests;

public class BuscarUserQueryHandlerTests
{
    [Fact]
    public async Task Handle_ComIdExistente_DevolveOUsuario()
    {
        var repositorio = new FakeUserRepository();
        repositorio.Usuarios["abc"] = new CustomUser
        {
            UserId = "abc",
            UserName = "maria",
            Email = "maria@teste.com"
        };
        var handler = new BuscarUserQueryHandler(repositorio);

        var usuario = await handler.Handle(new BuscarUserQuery("abc"), TestContext.Current.CancellationToken);

        Assert.NotNull(usuario);
        Assert.Equal("maria", usuario.UserName);
        Assert.Equal("maria@teste.com", usuario.Email);
    }

    [Fact]
    public async Task Handle_ComIdInexistente_DevolveNull()
    {
        var handler = new BuscarUserQueryHandler(new FakeUserRepository());

        var usuario = await handler.Handle(new BuscarUserQuery("nao-existe"), TestContext.Current.CancellationToken);

        Assert.Null(usuario);
    }

    [Fact]
    public async Task Handle_ComTokenJaCancelado_PropagaOperationCanceled()
    {
        var handler = new BuscarUserQueryHandler(new FakeUserRepository());
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            handler.Handle(new BuscarUserQuery("abc"), cts.Token));
    }
}