using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Domain.Tests;

public class RotaTests
{
    // ---------- Auxiliares: montam o cenário (a parte "Arrange") ----------
    private static Veiculo CriarVeiculo() =>
        new("Carro da empresa", "Fiat", "Uno", "ABC1D23", 1000m);

    private static Rota CriarRotaEmRascunho() =>
        new(CriarVeiculo(), 8, 2026, 120m, TipoRota.Funcionario, null);

    private static Rota CriarRotaAguardandoAprovacao()
    {
        var rota = CriarRotaEmRascunho();
        rota.EnviarParaAprovacao();
        return rota;
    }

    private static Rota CriarRotaAprovada()
    {
        var rota = CriarRotaAguardandoAprovacao();
        rota.Aprovar();
        return rota;
    }

    // ---------- Criação ----------
    [Fact]
    public void Construtor_ComDadosValidos_CriaRotaEmRascunho()
    {
        // Arrange
        var veiculo = CriarVeiculo();

        // Act
        var rota = new Rota(veiculo, 8, 2026, 120m, TipoRota.Focus, "Visita ao cliente");

        // Assert
        Assert.Equal(StatusRota.Rascunho, rota.Status);
        Assert.Equal(120m, rota.KmRodado);
        Assert.Equal(TipoRota.Focus, rota.Tipo);
        Assert.Equal("Visita ao cliente", rota.Complemento);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    [InlineData(-1)]
    public void Construtor_ComMesInvalido_LancaArgumentException(int mes)
    {
        Assert.Throws<ArgumentException>(() =>
            new Rota(CriarVeiculo(), mes, 2026, 100m, TipoRota.Funcionario, null));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Construtor_ComKmZeroOuNegativo_LancaArgumentException(int km)
    {
        Assert.Throws<ArgumentException>(() =>
            new Rota(CriarVeiculo(), 8, 2026, km, TipoRota.Funcionario, null));
    }

    // ---------- Rascunho: editar e enviar ----------
    [Fact]
    public void Editar_EmRascunho_AtualizaOsDados()
    {
        var rota = CriarRotaEmRascunho();

        rota.Editar(250m, TipoRota.Manutencao, "Troca de óleo");

        Assert.Equal(250m, rota.KmRodado);
        Assert.Equal(TipoRota.Manutencao, rota.Tipo);
        Assert.Equal("Troca de óleo", rota.Complemento);
    }

    [Fact]
    public void Editar_AposEnviarParaAprovacao_LancaInvalidOperationException()
    {
        var rota = CriarRotaAguardandoAprovacao();

        Assert.Throws<InvalidOperationException>(() =>
            rota.Editar(250m, TipoRota.Manutencao, null));
    }

    [Fact]
    public void EnviarParaAprovacao_EmRascunho_MudaParaAguardandoAprovacao()
    {
        var rota = CriarRotaEmRascunho();

        rota.EnviarParaAprovacao();

        Assert.Equal(StatusRota.AguardandoAprovacao, rota.Status);
    }

    [Fact]
    public void EnviarParaAprovacao_QuandoJaEnviada_LancaInvalidOperationException()
    {
        var rota = CriarRotaAguardandoAprovacao();

        Assert.Throws<InvalidOperationException>(() => rota.EnviarParaAprovacao());
    }

    // ---------- Aprovação e processamento ----------
    [Fact]
    public void Aprovar_AguardandoAprovacao_MudaParaAprovado()
    {
        var rota = CriarRotaAguardandoAprovacao();

        rota.Aprovar();

        Assert.Equal(StatusRota.Aprovado, rota.Status);
    }

    [Fact]
    public void Aprovar_EmRascunho_LancaInvalidOperationException()
    {
        var rota = CriarRotaEmRascunho();

        Assert.Throws<InvalidOperationException>(() => rota.Aprovar());
    }

    [Fact]
    public void MarcarComoProcessado_Aprovada_MudaParaProcessado()
    {
        var rota = CriarRotaAprovada();

        rota.MarcarComoProcessado();

        Assert.Equal(StatusRota.Processado, rota.Status);
    }

    [Fact]
    public void MarcarComoProcessado_AguardandoAprovacao_LancaInvalidOperationException()
    {
        var rota = CriarRotaAguardandoAprovacao();

        Assert.Throws<InvalidOperationException>(() => rota.MarcarComoProcessado());
    }

    // ---------- Rejeição ----------
    [Fact]
    public void Rejeitar_ComMotivo_MudaParaRejeitadoEGuardaOMotivo()
    {
        var rota = CriarRotaAguardandoAprovacao();

        rota.Rejeitar("KM incorreto");

        Assert.Equal(StatusRota.Rejeitado, rota.Status);
        Assert.Equal("KM incorreto", rota.MotivoRejeicao);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Rejeitar_SemMotivo_LancaArgumentException(string? motivo)
    {
        var rota = CriarRotaAguardandoAprovacao();

        // O "!" diz ao compilador "eu sei que pode ser null, é de propósito neste teste".
        Assert.Throws<ArgumentException>(() => rota.Rejeitar(motivo!));
    }

    [Fact]
    public void Rejeitar_EmRascunho_LancaInvalidOperationException()
    {
        var rota = CriarRotaEmRascunho();

        Assert.Throws<InvalidOperationException>(() => rota.Rejeitar("KM incorreto"));
    }
}