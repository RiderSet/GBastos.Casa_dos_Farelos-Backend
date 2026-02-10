using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Mappings;

public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Login)
            .IsRequired()
            .HasMaxLength(80);

        builder.Property(x => x.SenhaHash)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Perfil)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(x => x.Login).IsUnique();
    }
}