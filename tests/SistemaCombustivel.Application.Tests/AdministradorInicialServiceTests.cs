using SistemaCombustivel.Application.Tests.Fakes;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Tests;

public class AdministradorInicialServiceTests
{
    [Fact]
    public async Task GarantirAsync_SemAdministrador_CriaOAdministrador()
    {
        var cenario = new CenarioDeRegistro();

        var resultado = await cenario.AdministradorInicial.GarantirAsync(
            "Administrador", "admin@teste.com", "Senha123");

        Assert.True(resultado.Sucesso);
        var usuario = Assert.Single(cenario.Repositorio.Usuarios);
        Assert.Equal(TipoUsuario.Administrador, usuario.Tipo);
    }

    [Fact]
    public async Task GarantirAsync_ComAdministradorAtivo_NaoCriaNada()
    {
        var cenario = new CenarioDeRegistro();
        cenario.Repositorio.Usuarios.Add(
            new Usuario("Admin Antigo", "antigo@teste.com", TipoUsuario.Administrador));

        var resultado = await cenario.AdministradorInicial.GarantirAsync(
            "Administrador", "admin@teste.com", "Senha123");

        Assert.True(resultado.Sucesso);
        Assert.Single(cenario.Repositorio.Usuarios);
        Assert.Equal(0, cenario.UnitOfWork.TransacoesIniciadas);
    }

    [Fact]
    public async Task GarantirAsync_ComEmailDeUsuarioComumExistente_DevolveFalhaENaoCriaAdministrador()
    {
        var cenario = new CenarioDeRegistro();
        cenario.Repositorio.Usuarios.Add(
            new Usuario("Jackson", "jackson@teste.com", TipoUsuario.Comum));

        var resultado = await cenario.AdministradorInicial.GarantirAsync(
            "Administrador", "jackson@teste.com", "Senha123");

        Assert.False(resultado.Sucesso);
        Assert.DoesNotContain(cenario.Repositorio.Usuarios, u => u.Tipo == TipoUsuario.Administrador);
    }
}