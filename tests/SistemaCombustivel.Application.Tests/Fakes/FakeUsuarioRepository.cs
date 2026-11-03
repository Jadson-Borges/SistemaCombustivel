using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Tests.Fakes;

public class FakeUsuarioRepository : IUsuarioRepository
{
    public List<Usuario> Usuarios { get; } = new();
    public Exception? ExcecaoAoSalvar { get; set; }
    public List<CancellationToken> TokensRecebidos { get; } = new();

    // Imita o EF: anota o token recebido e falha se ele já estiver cancelado.
    private void Registrar(CancellationToken c)
    {
        TokensRecebidos.Add(c);
        c.ThrowIfCancellationRequested();
    }

    public Task AdicionarAsync(Usuario usuario, CancellationToken c)
    {
        Registrar(c);
        Usuarios.Add(usuario);
        return Task.CompletedTask;
    }

    public Task<bool> ExisteEmailAsync(string email, CancellationToken c)
    {
        Registrar(c);
        return Task.FromResult(Usuarios.Any(u =>
            string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase)));
    }

    public Task<bool> ExisteAdministradorAsync(CancellationToken c)
    {
        Registrar(c);
        return Task.FromResult(Usuarios.Any(u => u.Tipo == TipoUsuario.Administrador && u.Ativo));
    }

    public Task<Usuario?> ObterPorIdAsync(int id, CancellationToken c)
    {
        Registrar(c);
        return Task.FromResult(Usuarios.FirstOrDefault(u => u.Id == id));
    }

    public Task SalvarAlteracoesAsync(CancellationToken c)
    {
        Registrar(c);

        if (ExcecaoAoSalvar is not null)
            throw ExcecaoAoSalvar;

        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Usuario>> ListarAsync(CancellationToken c)
    {
        Registrar(c);
        return Task.FromResult<IReadOnlyList<Usuario>>(Usuarios.ToList());
    }
}