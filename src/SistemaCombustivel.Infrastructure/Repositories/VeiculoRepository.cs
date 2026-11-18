using Microsoft.EntityFrameworkCore;
using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Domain.Entities;
using SistemaCombustivel.Infrastructure.Data;

namespace SistemaCombustivel.Infrastructure.Repositories
{
    public class VeiculoRepository : IVeiculoRepository
    {
        private readonly AppDbContext _context;

        public VeiculoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Veiculo veiculo, CancellationToken c) =>
            await _context.Veiculos.AddAsync(veiculo, c);

        public async Task<bool> ExistePlacaAsync(string placaNormalizada, CancellationToken c) =>
            await _context.Veiculos.AnyAsync(v => v.Placa == placaNormalizada, c);

        public async Task<IReadOnlyList<Veiculo>> ListarTodosAsync(CancellationToken c) =>
            await _context.Veiculos.AsNoTracking().ToListAsync(c);

        public async Task<IReadOnlyList<Veiculo>> ListarDoUsuarioAsync(int usuarioId, CancellationToken c) =>
            await _context.Veiculos.AsNoTracking()
                .Where(v => v.Usuarios.Any(uv => uv.UsuarioId == usuarioId))
                .ToListAsync(c);

        public async Task SalvarAlteracoesAsync(CancellationToken c) =>
            await _context.SaveChangesAsync(c);
    }
}