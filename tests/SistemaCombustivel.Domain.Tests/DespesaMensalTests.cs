using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Domain.Tests;

public class DespesaMensalTests
{
    private static Veiculo CriarVeiculo() =>
        new("Carro da empresa", "Fiat", "Uno", "ABC1D23", 1000m);

    [Fact]
    public void Construtor_ComValoresValidos_CalculaTotalDeDespesas()
    {
        // Valores
        var despesa = new DespesaMensal(CriarVeiculo(), 8, 2026, 2000m, 500m, 1500m);

        Assert.Equal(4000m, despesa.TotalDespesas);
    }

    [Theory]
    [InlineData(-1, 0, 0)]
    [InlineData(0, -1, 0)]
    [InlineData(0, 0, -1)]
    public void Construtor_ComValorNegativo_LancaArgumentException(int abastecimento, int lavagem, int manutencao)
    {
        Assert.Throws<ArgumentException>(() =>
            new DespesaMensal(CriarVeiculo(), 8, 2026, abastecimento, lavagem, manutencao));
    }

    [Fact]
    public void AtualizarValores_ComValoresValidos_AtualizaOsValoresETotal()
    {
        var despesa = new DespesaMensal(CriarVeiculo(), 8, 2026, 2000m, 500m, 1500m);

        despesa.AtualizarValores(10m, 20m, 30m);

        Assert.Equal(10m, despesa.Abastecimento);
        Assert.Equal(20m, despesa.Lavagem);
        Assert.Equal(30m, despesa.Manutencao);
        Assert.Equal(60m, despesa.TotalDespesas);
    }

    [Fact]
    public void AtualizarValores_ComValorNegativo_LancaExcecaoEMantemOsValoresAnteriores()
    {
        var despesa = new DespesaMensal(CriarVeiculo(), 8, 2026, 2000m, 500m, 1500m);

        Assert.Throws<ArgumentException>(() => despesa.AtualizarValores(10m, -5m, 30m));

        Assert.Equal(2000m, despesa.Abastecimento);
        Assert.Equal(500m, despesa.Lavagem);
        Assert.Equal(1500m, despesa.Manutencao);
    }
}