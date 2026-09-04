using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaCombustivel.Domain.Entities
{
    internal class DespesaMensal
    {
        public int Id { get; private set; }
        public int VeiculoId { get; private set; }
        public Veiculo Veiculo { get; private set; } = null!;
        public int Mes { get; private set; }
        public int Ano { get; private set; }
        public decimal Abastecimento { get; private set; }
        public decimal Lavagem { get; private set; }
        public decimal Manutencao { get; private set; }

        public decimal TotalDespesas => Abastecimento + Lavagem + Manutencao;

        protected DespesaMensal() { }

        public DespesaMensal(Veiculo veiculo, int mes, int ano, decimal abastecimento, decimal lavagem, decimal manutencao)
        {
            if (mes is < 1 or > 12)
                throw new ArgumentException("Mês invalido.", nameof(mes));

            ValidadorValor(abastecimento, nameof(abastecimento));
            ValidadorValor(lavagem, nameof(lavagem));
            ValidadorValor(manutencao, nameof(manutencao));

            Veiculo = veiculo;
            VeiculoId = veiculo.Id;
            Mes = mes;
            Ano = ano;
            Abastecimento = abastecimento;
            Lavagem = lavagem;
            Manutencao = manutencao;
        }

        public void AtualizarValores(decimal abastecimento, decimal lavagem, decimal manutencao)
        {
            ValidadorValor(abastecimento, nameof(abastecimento));
            ValidadorValor(lavagem, nameof(lavagem));
            ValidadorValor(manutencao, nameof(manutencao));
        }

        private static void ValidadorValor(decimal valor, string nomeCampo)
        {
            if (valor < 0)
                throw new ArgumentException("O valor da despesa não pode ser negativo.");
        }
    }
}
