using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Mappings;

public class ItemCompraMapping : IEntityTypeConfiguration<ItemCompra>
{
    public void Configure(EntityTypeBuilder<ItemCompra> builder)
    {
        builder.ToTable("ItensCompra");

        //builder.HasKey(i => i.Id);

        builder.Property(i => i.ProdutoId)
               .IsRequired();

        builder.Property(i => i.Quantidade)
               .IsRequired();

        builder.Property(i => i.CustoUnitario)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(i => i.SubTotal)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.HasOne(i => i.Produto)
               .WithMany()
               .HasForeignKey(i => i.ProdutoId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Compra)
               .WithMany("_itens")
               .HasForeignKey(i => i.CompraId);
    }
}