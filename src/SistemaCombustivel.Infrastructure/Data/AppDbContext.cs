using Microsoft.EntityFrameworkCore;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<Veiculo> Veiculos => Set<Veiculo>();
        public DbSet<UsuarioVeiculo> UsuariosVeiculos => Set<UsuarioVeiculo>();
        public DbSet<Rota> Rotas => Set<Rota>();
        public DbSet<DespesaMensal> DespesasMensais => Set<DespesaMensal>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
