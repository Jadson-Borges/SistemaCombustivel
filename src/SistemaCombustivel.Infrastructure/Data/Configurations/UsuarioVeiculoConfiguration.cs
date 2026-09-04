using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Infrastructure.Data.Configurations
{
    public class UsuarioVeiculoConfiguration: IEntityTypeConfiguration<UsuarioVeiculo>
    {
        public void Configure(EntityTypeBuilder<UsuarioVeiculo> builder)
        {
            builder.ToTable("UsuariosVeiculos");

            builder.HasKey(uv => uv.Id);

            builder.HasOne(uv => uv.Usuario)
                .WithMany(u => u.Veiculos)
                .HasForeignKey(uv => uv.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(uv => uv.Veiculo)
                .WithMany(v => v.Usuarios)
                .HasForeignKey(uv => uv.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(uv => new { uv.UsuarioId, uv.VeiculoId }).IsUnique();
        }
    }
}
