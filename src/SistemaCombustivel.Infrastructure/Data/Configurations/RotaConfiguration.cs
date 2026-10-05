using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Infrastructure.Data.Configurations
{
    public class RotaConfiguration : IEntityTypeConfiguration<Rota>
    {
        public void Configure(EntityTypeBuilder<Rota> builder)
        {
            builder.ToTable("Rotas");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.KmRodado)
               .HasColumnType("decimal(18,2)");

            builder.Property(r => r.Tipo)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(r => r.Status)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(r => r.Complemento)
                .HasMaxLength(300);

            builder.Property(r => r.MotivoRejeicao)
                .HasMaxLength(500);

            builder.HasOne(r => r.Veiculo)
                .WithMany()
                .HasForeignKey(r => r.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => new { r.VeiculoId, r.Mes, r.Ano });
        }
    }
}
