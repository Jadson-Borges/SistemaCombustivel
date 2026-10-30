using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Abstractions
{
    public interface IUsuarioRepository
    {
        Task AdicionarAsync(Usuario usuario, CancellationToken c);
        Task<bool> ExisteEmailAsync(string email, CancellationToken c);
        Task<bool> ExisteAdministradorAsync(CancellationToken c);
        Task<Usuario?> ObterPorIdAsync(int id, CancellationToken c);
        Task SalvarAlteracoesAsync(CancellationToken c);
        Task<IReadOnlyList<Usuario>> ListarAsync(CancellationToken c);
    }
}