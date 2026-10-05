using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaCombustivel.Domain.Entities;

namespace SistemaCombustivel.Infrastructure.Data.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.NomeUsuario)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.Tipo)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(u =>u.ImagemPerfilUrl)
                .HasMaxLength(500);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Metadata
                .FindNavigation(nameof(Usuario.Veiculos))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);

        }
    }
}
