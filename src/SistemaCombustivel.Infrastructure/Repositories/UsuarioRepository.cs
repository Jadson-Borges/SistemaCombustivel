using Microsoft.EntityFrameworkCore;
using SistemaCombustivel.Application.Abstractions;
using SistemaCombustivel.Domain.Entities;
using SistemaCombustivel.Infrastructure.Data;

namespace SistemaCombustivel.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Usuario usuario, CancellationToken c) =>
            await _context.Usuarios.AddAsync(usuario, c);

        public async Task<bool> ExisteEmailAsync(string email, CancellationToken c) =>
            await _context.Usuarios.AnyAsync(u => u.Email == email, c);

        public async Task<bool> ExisteAdministradorAsync(CancellationToken c) =>
            await _context.Usuarios.AnyAsync(u => u.Tipo == TipoUsuario.Administrador && u.Ativo, c);

        public async Task SalvarAlteracoesAsync(CancellationToken c) =>
            await _context.SaveChangesAsync(c);

        public async Task<Usuario?> ObterPorIdAsync(int id, CancellationToken c) =>
            await _context.Usuarios.FindAsync(new object[] { id }, c);

        public async Task<IReadOnlyList<Usuario>> ListarAsync(CancellationToken c) =>
            await _context.Usuarios.AsNoTracking().ToListAsync(c);
    }
}