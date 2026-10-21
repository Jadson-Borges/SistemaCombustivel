using SistemaCombustivel.Application.Tests.Fakes;
using SistemaCombustivel.Application.Usuarios.Commands.CriarAdministradorInicial;
using SistemaCombustivel.Application.Usuarios.Commands.CriarUsuario;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Tests;

public class CancelamentoTests
{
    private static CriarUsuarioCommand ComandoCriar() =>
        new("Maria", "maria@teste.com", "Senha123", TipoUsuario.Comum);

    [Fact]
    public async Task CriarUsuario_RepassaOMesmoTokenAoRepositorioEATransacao()
    {
        var cenario = new CenarioDeRegistro();
        using var cts = new CancellationTokenSource();

        await cenario.CriarUsuario.Handle(ComandoCriar(), cts.Token);

        Assert.NotEmpty(cenario.Repositorio.TokensRecebidos);
        Assert.All(cenario.Repositorio.TokensRecebidos, t => Assert.Equal(cts.Token, t));
        var tokenDaTransacao = Assert.Single(cenario.UnitOfWork.TokensRecebidos);
        Assert.Equal(cts.Token, tokenDaTransacao);
    }

    [Fact]
    public async Task CriarUsuario_ComTokenJaCancelado_LancaOperationCanceledENaoGravaNada()
    {
        var cenario = new CenarioDeRegistro();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            cenario.CriarUsuario.Handle(ComandoCriar(), cts.Token));

        Assert.Empty(cenario.Repositorio.Usuarios);
        Assert.Empty(cenario.Autenticacao.Registros);
    }

    [Fact]
    public async Task CriarAdministradorInicial_RepassaOTokenAoVerificarSeJaExisteAdministrador()
    {
        var cenario = new CenarioDeRegistro();
        using var cts = new CancellationTokenSource();

        await cenario.AdministradorInicial.Handle(
            new CriarAdministradorInicialCommand("Administrador", "admin@teste.com", "Senha123"), cts.Token);

        Assert.All(cenario.Repositorio.TokensRecebidos, t => Assert.Equal(cts.Token, t));
    }
}