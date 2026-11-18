using SistemaCombustivel.Application.Tests.Fakes;
using SistemaCombustivel.Application.Veiculos.Commands.CadastrarVeiculo;

namespace SistemaCombustivel.Application.Tests;

public class CadastrarVeiculoCommandHandlerTests
{
    private readonly FakeVeiculoRepository _veiculos = new();
    private readonly FakeUsuarioRepository _usuarios = new();

    private CadastrarVeiculoCommandHandler CriarHandler() => new(_veiculos, _usuarios);

    private static CadastrarVeiculoCommand Comando(int usuarioId = 1, string placa = "abc-1d23") =>
        new(usuarioId, "Carro operacional", "Fiat", "Uno", placa, 1000m);

    [Fact]
    public async Task Handle_DadosValidos_GravaVeiculoNormalizadoVinculadoAoUsuario()
    {
        var maria = UsuarioDeTeste.Criar(1, "Maria");
        _usuarios.Usuarios.Add(maria);

        var resultado = await CriarHandler().Handle(Comando(), CancellationToken.None);

        Assert.True(resultado.Sucesso);
        var veiculo = Assert.Single(_veiculos.Veiculos);
        Assert.Equal("ABC1D23", veiculo.Placa);
        Assert.Same(maria, Assert.Single(veiculo.Usuarios).Usuario);
        Assert.Equal(1, _veiculos.Salvamentos);
    }

    [Fact]
    public async Task Handle_PlacaJaCadastradaEmOutroFormato_Falha()
    {
        _usuarios.Usuarios.Add(UsuarioDeTeste.Criar(1, "Maria"));
        var handler = CriarHandler();
        await handler.Handle(Comando(placa: "ABC1D23"), CancellationToken.None);

        var resultado = await handler.Handle(Comando(placa: "abc-1d23"), CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Single(_veiculos.Veiculos);
    }

    [Fact]
    public async Task Handle_PlacaInvalida_FalhaSemGravar()
    {
        _usuarios.Usuarios.Add(UsuarioDeTeste.Criar(1, "Maria"));

        var resultado = await CriarHandler().Handle(Comando(placa: "123"), CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Empty(_veiculos.Veiculos);
    }

    [Fact]
    public async Task Handle_UsuarioInexistente_Falha()
    {
        var resultado = await CriarHandler().Handle(Comando(usuarioId: 99), CancellationToken.None);

        Assert.False(resultado.Sucesso);
        Assert.Empty(_veiculos.Veiculos);
    }
}