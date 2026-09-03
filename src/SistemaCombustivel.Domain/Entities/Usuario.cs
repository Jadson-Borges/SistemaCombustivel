
namespace SistemaCombustivel.Domain.Entities
{
    internal class Usuario
    {
        public int Id { get; private set; }
        public string NomeUsuario { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public TipoUsuario Tipo { get; private set; }
        public bool Ativo { get; private set; } = true;
        public string? ImagemPerfilUrl { get; private set; }

        private readonly List<UsuarioVeiculo> _veiculos = new();
        public IReadOnlyCollection<UsuarioVeiculo> Veiculos => _veiculos.AsReadOnly();

        //usado pelo Etity Framework
        protected Usuario() 
        {
        }

        public Usuario(string nomeUsuario, string email, TipoUsuario tipo)
        {
            NomeUsuario = nomeUsuario;
            Email = email;
            Tipo = tipo;
        }

        public void AtualizarPerfil(string nomeUsuario, string email, string? imagemPerfilUrl)
        {
            NomeUsuario = nomeUsuario;
            Email = email;
            ImagemPerfilUrl = imagemPerfilUrl;
        }

        public void AlterarTipo(TipoUsuario novoTipo)
        {
            Tipo = novoTipo;
        }

        public void Desativar() => Ativo = false;
        public void Reativar() => Ativo = true;
    }
}
