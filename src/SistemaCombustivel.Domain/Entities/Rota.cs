namespace SistemaCombustivel.Domain.Entities
{
    public class Rota
    {
        public int Id { get; private set; }
        public int VeiculoId { get; private set; }
        public Veiculo Veiculo { get; private set; } = null!;
        public int Mes { get; private set; }
        public int Ano { get; private set; }
        public decimal KmRodado { get; private set; }
        public TipoRota Tipo { get; private set; }
        public string? Complemento { get; private set; }
        public StatusRota Status { get; private set; }
        public string? MotivoRejeicao { get; private set; }

        protected Rota() { }

        public Rota(Veiculo veiculo, int mes, int ano, decimal kmRodado, TipoRota tipo, string? complemento)
        {
            if (mes is < 1 or > 12)
                throw new ArgumentException("Mês inválido.", nameof(mes));

            if (kmRodado <= 0)
                throw new ArgumentException("A quilometragem rodada deve ser maior que zero", nameof(kmRodado));
            Veiculo = veiculo;
            VeiculoId = veiculo.Id;
            Mes = mes;
            Ano = ano;
            KmRodado = kmRodado;
            Tipo = tipo;
            Complemento = complemento;
            Status = StatusRota.Rascunho;
        }

        public void Editar(decimal kmRodado, TipoRota tipo, string? complemento)
        {
            GarantirQueEstaEm(StatusRota.Rascunho, "editar");

            if (kmRodado <= 0)
                throw new ArgumentException("A quilometragem rodada deve ser maior que zero.", nameof(kmRodado));

            KmRodado = kmRodado;
            Tipo = tipo;
            Complemento = complemento;

        }

        public void EnviarParaAprovacao()
        {
            GarantirQueEstaEm(StatusRota.Rascunho, "enviar para aprovação");
            Status = StatusRota.AguardandoAprovacao;
        }

        public void Aprovar()
        {
            GarantirQueEstaEm(StatusRota.AguardandoAprovacao, "aprovar");
            Status = StatusRota.Aprovado;
        }

        public void Rejeitar(string motivo)
        {
            GarantirQueEstaEm(StatusRota.AguardandoAprovacao, "rejeitar");

            if (!string.IsNullOrEmpty(motivo))
                throw new ArgumentException("O motivo da rejeição é obrigatorio.");

            Status = StatusRota.Rejeitado;
            MotivoRejeicao = motivo;
        }

        public void MarcarComoProcessado()
        {
            GarantirQueEstaEm(StatusRota.Aprovado, "processar");
            Status = StatusRota.Processado;
        }

        private void GarantirQueEstaEm(StatusRota statusEsperando, string acao)
        {
            if (Status != statusEsperando)
                throw new InvalidOperationException(
                    $"Não é possivel {acao} uma rota que está com status '{Status}'. " +
                    $"Esperando: '{statusEsperando}'.");

        }


    }
}
