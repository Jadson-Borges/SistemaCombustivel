using SistemaCombustivel.Application.Tests.Fakes;
using SistemaCombustivel.Application.Usuarios.Commands.CriarAdministradorInicial;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Tests;

public class CriarAdministradorInicialCommandHandlerTests
{
    private static CriarAdministradorInicialCommand Comando(string email = "admin@teste.com") =>
        new("Administrador", email, "Senha123");

    [Fact]
    public async Task Handle_SemAdministrador_CriaOAdministrador()
    {
        var cenario = new CenarioDeRegistro();

        var resultado = await cenario.AdministradorInicial.Handle(Comando(), CancellationToken.None);

        Assert.True(resultado.Sucesso);
        var usuario = Assert.Single(cenario.Repositorio.Usuarios);
        Assert.Equal(TipoUsuario.Administrador, usuario.Tipo);
    }

    [Fact]
    public async Task Handle_ComAdministradorAtivo_NaoCriaNada()
    {
        var cenario = new CenarioDeRegistro();
        cenario.Repositorio.Usuarios.Add(
            new Usuario("Admin Antigo", "antigo@teste.com", TipoUsuario.Administrador));

        var resultado = await cenario.AdministradorInicial.Handle(Comando(), CancellationToken.None);

        Assert.True(resultado.Sucesso);
        Assert.Single(cenario.Repositorio.Usuarios);
        Assert.Equal(0, cenario.UnitOfWork.TransacoesIniciadas);
    }

    [Fact]
    public async Task Handle_ComEmailDeUsuarioComumExistente_DevolveFalhaENaoCriaAdministrador()
    {
        var cenario = new CenarioDeRegistro();
        cenario.Repositorio.Usuarios.Add(new Usuario("Jackson", "jackson@teste.com", TipoUsuario.Comum));

        var resultado = await cenario.AdministradorInicial.Handle(Comando("jackson@teste.com"), CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.DoesNotContain(cenario.Repositorio.Usuarios, u => u.Tipo == TipoUsuario.Administrador);
    }
}