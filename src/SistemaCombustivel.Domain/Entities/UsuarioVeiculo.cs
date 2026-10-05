
namespace SistemaCombustivel.Domain.Entities
{
    public class UsuarioVeiculo
    {
        public int Id { get; private set; }
        public int UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; } = null!;
        public int VeiculoId { get; private set; }
        public Veiculo Veiculo { get; private set; } = null!;
        public DateTime DataVinculo { get; private set; }

        protected UsuarioVeiculo() { }

        public UsuarioVeiculo(Usuario usuario, Veiculo veiculo)
        {
            Usuario = usuario;
            Veiculo = veiculo;
            DataVinculo = DateTime.UtcNow;
        }
    }
}
