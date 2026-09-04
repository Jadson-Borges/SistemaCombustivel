using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCombustivel.Domain.Entities;


namespace SistemaCombustivel.Infrastructure.Data.Configurations
{
    public class VeiculoConfiguration : IEntityTypeConfiguration<Veiculo>
    {
        public void Configure(EntityTypeBuilder <Veiculo> builder)
        {
            builder.ToTable("Veiculos");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Descricao)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(v => v.Marca)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(v => v.Modelo)
                .IsRequired()
                .HasMaxLength (50);

            builder.Property(v => v.Placa)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(v => v.KmInicial)
                .HasColumnType("decimal(18,2)");

            builder.Property(v => v.KmOficial)
                .HasColumnType("decimal(18,2)");

            builder.HasIndex(v => v.Placa)
                .IsUnique();

            builder.Metadata
                .FindNavigation(nameof(Veiculo.Usuarios))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
