using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Mappings;

public class ClientePJMapping : IEntityTypeConfiguration<ClientePJ>
{
    public void Configure(EntityTypeBuilder<ClientePJ> builder)
    {
        builder.Property(x => x.CNPJ)
            .HasColumnName("CNPJ")
            .HasMaxLength(14)
            .IsRequired();

        builder.Property(x => x.Contato)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(x => x.CNPJ)
            .IsUnique();
    }
}
