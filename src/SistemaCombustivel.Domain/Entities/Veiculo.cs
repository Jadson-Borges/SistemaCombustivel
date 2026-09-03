namespace SistemaCombustivel.Domain.Entities
{
    public class Veiculo
    {
        public int Id { get; private set; }
        public string Descricao { get; private set; } = string.Empty;
        public string Marca { get; private set; } = string.Empty;
        public string Modelo { get; private set; } = string.Empty;
        public string Placa { get; private set; } = string.Empty;
        public decimal KmInicial { get; private set; }
        public decimal KmOficial { get; private set; }
        public bool Ativo { get; private set; } = true;

        private readonly List<UsuarioVeiculo> _usuarios = new();
        public IReadOnlyCollection<UsuarioVeiculo> Usuarios => _usuarios.AsReadOnly();

        protected Veiculo() { }

        public Veiculo(string descricao, string marca, string modelo, string placa, decimal kmInicial)
        {
            if (kmInicial < 0)

                throw new ArgumentException("A quilometragem inicial não pode ser negativa.", nameof(kmInicial));

            Descricao = descricao;
            Marca = marca;
            Modelo = modelo;
            Placa = placa;
            KmInicial = kmInicial;
            KmOficial = kmInicial;
        }

        public void AdicionarUsuario(Usuario usuario)
        {
            var vinculo = new UsuarioVeiculo(usuario, this);
            _usuarios.Add(vinculo);
            usuario.RegistrarVinculo(vinculo);
        }
        public void AtualizarKmOficial(decimal kmRotasAprovadas)
        {
            if (kmRotasAprovadas < 0)
                throw new ArgumentException("A soma de KM aprovado não pode ser negativa", nameof(kmRotasAprovadas));

            KmOficial = KmInicial + kmRotasAprovadas;
        }

        public void Desativar() => Ativo = false;
        public void Reativar() => Ativo = true;
    }
}

