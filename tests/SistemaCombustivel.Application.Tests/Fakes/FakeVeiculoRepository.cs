using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Application.Tests.Fakes;

public class FakeVeiculoRepository : IVeiculoRepository
{
    public List<Veiculo> Veiculos { get; } = new();
    public int Salvamentos { get; private set; }

    public Task AdicionarAsync(Veiculo veiculo, CancellationToken c)
    {
        Veiculos.Add(veiculo);
        return Task.CompletedTask;
    }

    public Task<bool> ExistePlacaAsync(string placaNormalizada, CancellationToken c) =>
        Task.FromResult(Veiculos.Any(v => v.Placa == placaNormalizada));

    public Task<IReadOnlyList<Veiculo>> ListarTodosAsync(CancellationToken c) =>
        Task.FromResult<IReadOnlyList<Veiculo>>(Veiculos.ToList());

    public Task<IReadOnlyList<Veiculo>> ListarDoUsuarioAsync(int usuarioId, CancellationToken c) =>
        Task.FromResult<IReadOnlyList<Veiculo>>(
            Veiculos.Where(v => v.Usuarios.Any(uv => uv.Usuario.Id == usuarioId)).ToList());

    public Task SalvarAlteracoesAsync(CancellationToken c)
    {
        Salvamentos++;
        return Task.CompletedTask;
    }
}