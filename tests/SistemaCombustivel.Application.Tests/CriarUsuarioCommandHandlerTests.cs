using SistemaCombustivel.Application.Tests.Fakes;
using SistemaCombustivel.Application.Usuarios.Commands.CriarUsuario;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Tests;

public class CriarUsuarioCommandHandlerTests
{
    private static CriarUsuarioCommand Comando(
        string email = "maria@teste.com", TipoUsuario tipo = TipoUsuario.Comum) =>
        new("Maria", email, "Senha123", tipo);

    [Fact]
    public async Task Handle_ComTipoAdministrador_CriaUsuarioAdministrador()
    {
        var cenario = new CenarioDeRegistro();

        var resultado = await cenario.CriarUsuario.Handle(
            Comando("admin@teste.com", TipoUsuario.Administrador), CancellationToken.None);

        Assert.True(resultado.Sucesso);
        var usuario = Assert.Single(cenario.Repositorio.Usuarios);
        Assert.Equal(TipoUsuario.Administrador, usuario.Tipo);
    }

    [Fact]
    public async Task Handle_ComDadosValidos_RegistraCredenciaisEConfirmaATransacao()
    {
        var cenario = new CenarioDeRegistro();

        await cenario.CriarUsuario.Handle(Comando(), CancellationToken.None);

        var registro = Assert.Single(cenario.Autenticacao.Registros);
        Assert.Equal("maria@teste.com", registro.Email);
        Assert.Equal("Senha123", registro.Senha);
        Assert.Equal(1, cenario.UnitOfWork.Commits);
        Assert.Equal(0, cenario.UnitOfWork.Rollbacks);
    }

    [Fact]
    public async Task Handle_ComEmailJaCadastrado_DevolveFalhaESemAbrirTransacao()
    {
        var cenario = new CenarioDeRegistro();
        cenario.Repositorio.Usuarios.Add(new Usuario("João", "maria@teste.com", TipoUsuario.Comum));

        var resultado = await cenario.CriarUsuario.Handle(Comando(), CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Contains("e-mail", resultado.Erro);
        Assert.Equal(0, cenario.UnitOfWork.TransacoesIniciadas);
        Assert.Empty(cenario.Autenticacao.Registros);
    }

    [Fact]
    public async Task Handle_QuandoAAutenticacaoFalha_DevolveFalhaEDesfazATransacao()
    {
        var cenario = new CenarioDeRegistro();
        cenario.Autenticacao.FalharNoRegistro = true;
        cenario.Autenticacao.MensagemDeErro = "Senha fraca.";

        var resultado = await cenario.CriarUsuario.Handle(Comando(), CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Equal("Senha fraca.", resultado.Erro);
        Assert.Equal(1, cenario.UnitOfWork.Rollbacks);
        Assert.Equal(0, cenario.UnitOfWork.Commits);
    }

    [Fact]
    public async Task Handle_QuandoOcorreErroInesperado_PropagaAExcecaoEmVezDeEngoli()
    {
        var cenario = new CenarioDeRegistro();
        cenario.Repositorio.ExcecaoAoSalvar = new InvalidOperationException("falha de banco");

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            cenario.CriarUsuario.Handle(Comando(), CancellationToken.None));

        Assert.Equal(1, cenario.UnitOfWork.Rollbacks);
    }
}