using SistemaCombustivel.Domain.Entities;
using Xunit;

namespace SistemaCombustivel.Domain.Tests;

public class VeiculoTests
{
    [Theory]
    [InlineData("abc-1d23", "ABC1D23")]
    [InlineData(" ABC 1234 ", "ABC1234")]
    public void NormalizarPlaca_RemoveSeparadoresEPadronizaMaiusculas(string entrada, string esperado) =>
        Assert.Equal(esperado, Veiculo.NormalizarPlaca(entrada));

    [Theory]
    [InlineData("ABC1234", true)]
    [InlineData("abc-1d23", true)]
    [InlineData("AB12345", false)]
    [InlineData("", false)]
    public void PlacaEhValida_ReconheceFormatosAntigoEMercosul(string placa, bool esperado) =>
        Assert.Equal(esperado, Veiculo.PlacaEhValida(placa));

    [Fact]
    public void Construtor_DadosValidos_NormalizaPlacaEIniciaKmOficialIgualAoInicial()
    {
        var veiculo = new Veiculo("Carro operacional", "Fiat", "Uno", "abc-1d23", 1500m);

        Assert.Equal("ABC1D23", veiculo.Placa);
        Assert.Equal(1500m, veiculo.KmOficial);
        Assert.True(veiculo.Ativo);
    }

    [Fact]
    public void Construtor_KmInicialNegativo_LancaExcecao() =>
        Assert.Throws<ArgumentException>(() => new Veiculo("Carro", "Fiat", "Uno", "ABC1234", -1m));

    [Fact]
    public void Construtor_PlacaInvalida_LancaExcecao() =>
        Assert.Throws<ArgumentException>(() => new Veiculo("Carro", "Fiat", "Uno", "123", 0m));
}