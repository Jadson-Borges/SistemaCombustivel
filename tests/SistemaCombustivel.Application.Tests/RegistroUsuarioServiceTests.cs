using SistemaCombustivel.Application.Tests.Fakes;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Tests;

public class RegistroUsuarioServiceTests
{
    [Fact]
    public async Task RegistrarAsync_ComDadosValidos_CriaUsuarioComum()
    {
        // Arrange
        var cenario = new CenarioDeRegistro();

        // Act
        var resultado = await cenario.Registro.RegistrarAsync("Maria", "maria@teste.com", "Senha123");

        // Assert
        Assert.True(resultado.Sucesso);
        var usuario = Assert.Single(cenario.Repositorio.Usuarios);
        Assert.Equal(TipoUsuario.Comum, usuario.Tipo);
        Assert.Equal("maria@teste.com", usuario.Email);
    }

    [Fact]
    public async Task CriarAsync_ComTipoAdministrador_CriaUsuarioAdministrador()
    {
        var cenario = new CenarioDeRegistro();

        var resultado = await cenario.Registro.CriarAsync(
            "Admin", "admin@teste.com", "Senha123", TipoUsuario.Administrador);

        Assert.True(resultado.Sucesso);
        var usuario = Assert.Single(cenario.Repositorio.Usuarios);
        Assert.Equal(TipoUsuario.Administrador, usuario.Tipo);
    }

    [Fact]
    public async Task RegistrarAsync_ComDadosValidos_RegistraCredenciaisEConfirmaATransacao()
    {
        var cenario = new CenarioDeRegistro();

        await cenario.Registro.RegistrarAsync("Maria", "maria@teste.com", "Senha123");

        var registro = Assert.Single(cenario.Autenticacao.Registros);
        Assert.Equal("maria@teste.com", registro.Email);
        Assert.Equal("Senha123", registro.Senha);
        Assert.Equal(1, cenario.UnitOfWork.Commits);
        Assert.Equal(0, cenario.UnitOfWork.Rollbacks);
    }

    [Fact]
    public async Task RegistrarAsync_ComEmailJaCadastrado_DevolveFalhaESemAbrirTransacao()
    {
        var cenario = new CenarioDeRegistro();
        cenario.Repositorio.Usuarios.Add(new Usuario("João", "joao@teste.com", TipoUsuario.Comum));

        var resultado = await cenario.Registro.RegistrarAsync("Outro João", "joao@teste.com", "Senha123");

        Assert.False(resultado.Sucesso);
        Assert.Contains("e-mail", resultado.Erro);
        Assert.Equal(0, cenario.UnitOfWork.TransacoesIniciadas);
        Assert.Empty(cenario.Autenticacao.Registros);
    }

    [Fact]
    public async Task RegistrarAsync_QuandoAAutenticacaoFalha_DevolveFalhaEDesfazATransacao()
    {
        var cenario = new CenarioDeRegistro();
        cenario.Autenticacao.FalharNoRegistro = true;
        cenario.Autenticacao.MensagemDeErro = "Senha fraca.";

        var resultado = await cenario.Registro.RegistrarAsync("Maria", "maria@teste.com", "123");

        Assert.False(resultado.Sucesso);
        Assert.Equal("Senha fraca.", resultado.Erro);
        Assert.Equal(1, cenario.UnitOfWork.Rollbacks);
        Assert.Equal(0, cenario.UnitOfWork.Commits);
    }

    [Fact]
    public async Task RegistrarAsync_QuandoOcorreErroInesperado_PropagaAExcecaoEmVezDeEngoli()
    {
        var cenario = new CenarioDeRegistro();
        cenario.Repositorio.ExcecaoAoSalvar = new InvalidOperationException("falha de banco");

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            cenario.Registro.RegistrarAsync("Maria", "maria@teste.com", "Senha123"));

        Assert.Equal(1, cenario.UnitOfWork.Rollbacks);
    }
}