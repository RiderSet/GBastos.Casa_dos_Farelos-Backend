using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Configuration;

public sealed class ItemCompraConfiguration : IEntityTypeConfiguration<ItemCompra>
{
    public void Configure(EntityTypeBuilder<ItemCompra> builder)
    {
        builder.ToTable("ItensCompra");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.NomeProduto).IsRequired();
        builder.Property(i => i.Quantidade).IsRequired();
        builder.Property(i => i.CustoUnitario).HasPrecision(18, 2);
    }
}