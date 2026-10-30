using SistemaCombustivel.Application.Tests.Fakes;
using SistemaCombustivel.Application.Usuarios.Queries.ListarUsuarios;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Tests;

public class ListarUsuariosQueryHandlerTests
{
    [Fact]
    public async Task Handle_ComUsuariosCadastrados_RetornaOrdenadosPorNome()
    {
        var repositorio = new FakeUsuarioRepository();
        repositorio.Usuarios.Add(new Usuario("Zeca", "zeca@teste.com", TipoUsuario.Comum));
        repositorio.Usuarios.Add(new Usuario("Ana", "ana@teste.com", TipoUsuario.Administrador));
        var handler = new ListarUsuariosQueryHandler(repositorio);

        var lista = await handler.Handle(new ListarUsuariosQuery(), CancellationToken.None);

        Assert.Equal(new[] { "Ana", "Zeca" }, lista.Select(u => u.NomeUsuario));
    }
}