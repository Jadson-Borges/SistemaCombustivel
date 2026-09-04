using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Infrastructure.Data.Configurations
{
    public class DespesaMensalConfiguration : IEntityTypeConfiguration<DespesaMensal>
    {
        public void Configure(EntityTypeBuilder<DespesaMensal> builder)
        {
            builder.ToTable("DespesasMensais");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Abastecimento)
                .HasColumnType("decimal(18,2)");

            builder.Property(d => d.Lavagem)
                .HasColumnType("decimal(18,2)");

            builder.Property(d => d.Manutencao)
                .HasColumnType("decimal(18,2)");

            builder.Ignore(d => d.TotalDespesas);

            builder.HasOne(d => d.Veiculo)
                .WithMany()
                .HasForeignKey(d => d.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(d => new { d.VeiculoId, d.Mes, d.Ano }).IsUnique();
        }
    }
}