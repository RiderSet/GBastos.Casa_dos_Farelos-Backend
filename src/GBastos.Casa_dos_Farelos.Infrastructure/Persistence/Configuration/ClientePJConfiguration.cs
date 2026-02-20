using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Configuration;

public sealed class ClientePJConfiguration : IEntityTypeConfiguration<ClientePJ>
{
    public void Configure(EntityTypeBuilder<ClientePJ> builder)
    {
        builder.Property(x => x.CNPJ)
            .IsRequired()
            .HasMaxLength(14);

        builder.HasIndex(x => x.CNPJ)
            .IsUnique();

        builder.Property(x => x.RazaoSocial)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.NomeFantasia)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Contato)
            .HasMaxLength(150);

        builder.Property(x => x.Telefone)
            .HasMaxLength(20);

        builder.Property(x => x.Email)
            .HasMaxLength(150);
    }
}
