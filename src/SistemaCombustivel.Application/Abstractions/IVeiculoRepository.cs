using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Abstractions;

public interface IVeiculoRepository
{
    Task AdicionarAsync(Veiculo veiculo, CancellationToken c);
    Task<bool> ExistePlacaAsync(string placaNormalizada, CancellationToken c);
    Task<IReadOnlyList<Veiculo>> ListarTodosAsync(CancellationToken c);
    Task<IReadOnlyList<Veiculo>> ListarDoUsuarioAsync(int usuarioId, CancellationToken c);
    Task SalvarAlteracoesAsync(CancellationToken c);
}