using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Abstractions
{
    public interface IUsuarioRepository
    {
        Task AdicionarAsync(Usuario usuario);
        Task<bool> ExisteEmailAsync(string email);
        Task SalvarAlteracoesAsync();
    }
}