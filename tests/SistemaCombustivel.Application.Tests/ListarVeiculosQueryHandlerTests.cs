using SistemaCombustivel.Application.Tests.Fakes;
using SistemaCombustivel.Application.Veiculos.Queries.ListarVeiculos;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Tests;

public class ListarVeiculosQueryHandlerTests
{
    private readonly FakeVeiculoRepository _repositorio = new();

    public ListarVeiculosQueryHandlerTests()
    {
        var maria = UsuarioDeTeste.Criar(1, "Maria");
        var joao = UsuarioDeTeste.Criar(2, "Joao");

        var carroDaMaria = new Veiculo("Carro da Maria", "Fiat", "Uno", "AAA1111", 0m);
        carroDaMaria.AdicionarUsuario(maria);
        var carroDoJoao = new Veiculo("Carro do Joao", "VW", "Gol", "BBB2222", 0m);
        carroDoJoao.AdicionarUsuario(joao);

        _repositorio.Veiculos.AddRange(new[] { carroDaMaria, carroDoJoao });
    }

    [Fact]
    public async Task Handle_UsuarioComum_VeSomenteOsVeiculosVinculados()
    {
        var handler = new ListarVeiculosQueryHandler(_repositorio);

        var lista = await handler.Handle(new ListarVeiculosQuery(1, VerTodos: false), CancellationToken.None);

        Assert.Equal(new[] { "AAA1111" }, lista.Select(v => v.Placa));
    }

    [Fact]
    public async Task Handle_Administrador_VeTodos()
    {
        var handler = new ListarVeiculosQueryHandler(_repositorio);

        var lista = await handler.Handle(new ListarVeiculosQuery(1, VerTodos: true), CancellationToken.None);

        Assert.Equal(2, lista.Count);
    }
}