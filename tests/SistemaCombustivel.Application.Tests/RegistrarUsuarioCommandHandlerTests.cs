using SistemaCombustivel.Application.Tests.Fakes;
using SistemaCombustivel.Application.Usuarios.Commands.RegistrarUsuario;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Tests;

public class RegistrarUsuarioCommandHandlerTests
{
    [Fact]
    public async Task Handle_ComDadosValidos_CriaUsuarioComum()
    {
        var cenario = new CenarioDeRegistro();

        var resultado = await cenario.Registrar.Handle(
            new RegistrarUsuarioCommand("Maria", "maria@teste.com", "Senha123"), CancellationToken.None);

        Assert.True(resultado.Sucesso);
        var usuario = Assert.Single(cenario.Repositorio.Usuarios);
        Assert.Equal(TipoUsuario.Comum, usuario.Tipo);
    }
}