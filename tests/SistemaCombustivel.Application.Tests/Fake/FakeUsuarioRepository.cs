using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Tests.Fakes;

public class FakeUsuarioRepository : IUsuarioRepository
{
    public List<Usuario> Usuarios { get; } = new();

    // Permite ao teste simular uma falha inesperada ao gravar.
    public Exception? ExcecaoAoSalvar { get; set; }

    public Task AdicionarAsync(Usuario usuario)
    {
        Usuarios.Add(usuario);
        return Task.CompletedTask;
    }

    public Task<bool> ExisteEmailAsync(string email) =>
        Task.FromResult(Usuarios.Any(u =>
            string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase)));

    public Task<bool> ExisteAdministradorAsync() =>
        Task.FromResult(Usuarios.Any(u => u.Tipo == TipoUsuario.Administrador && u.Ativo));

    public Task<Usuario?> ObterPorIdAsync(int id) =>
        Task.FromResult(Usuarios.FirstOrDefault(u => u.Id == id));

    public Task SalvarAlteracoesAsync()
    {
        if (ExcecaoAoSalvar is not null)
            throw ExcecaoAoSalvar;

        return Task.CompletedTask;
    }
}