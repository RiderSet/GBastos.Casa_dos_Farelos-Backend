using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Mappings;

public class ClientePFMapping : IEntityTypeConfiguration<ClientePF>
{
    public void Configure(EntityTypeBuilder<ClientePF> builder)
    {
        builder.Property(x => x.CPF)
            .HasColumnName("CPF")
            .HasMaxLength(11)
            .IsRequired();

        builder.HasIndex(x => x.CPF)
            .IsUnique();
    }
}
