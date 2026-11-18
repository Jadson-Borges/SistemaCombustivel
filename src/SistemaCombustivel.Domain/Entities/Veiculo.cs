using System.Text.RegularExpressions;

namespace SistemaCombustivel.Domain.Entities
{
    public class Veiculo
    {
        // Placa antiga (ABC1234) ou Mercosul (ABC1D23).
        private static readonly Regex FormatoPlaca = new("^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$", RegexOptions.Compiled);

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
            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Informe a identificação do veículo.", nameof(descricao));
            if (string.IsNullOrWhiteSpace(marca))
                throw new ArgumentException("Informe a marca do veículo.", nameof(marca));
            if (string.IsNullOrWhiteSpace(modelo))
                throw new ArgumentException("Informe o modelo do veículo.", nameof(modelo));
            if (!PlacaEhValida(placa))
                throw new ArgumentException("Placa inválida. Use o formato ABC1234 ou ABC1D23.", nameof(placa));
            if (kmInicial < 0)
                throw new ArgumentException("A quilometragem inicial não pode ser negativa.", nameof(kmInicial));

            Descricao = descricao.Trim();
            Marca = marca.Trim();
            Modelo = modelo.Trim();
            Placa = NormalizarPlaca(placa);
            KmInicial = kmInicial;
            KmOficial = kmInicial;
        }

        // Deixa só letras e números, em maiúsculas: "abc-1d23" vira "ABC1D23".
        public static string NormalizarPlaca(string? placa) =>
            new string((placa ?? string.Empty).Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();

        public static bool PlacaEhValida(string? placa) =>
            FormatoPlaca.IsMatch(NormalizarPlaca(placa));

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