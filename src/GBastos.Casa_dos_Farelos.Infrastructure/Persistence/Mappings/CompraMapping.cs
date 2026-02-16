using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Mappings;

public class CompraMapping : IEntityTypeConfiguration<Compra>
{
    public void Configure(EntityTypeBuilder<Compra> builder)
    {
        builder.ToTable("Compras");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.DataCompra)
               .IsRequired();

        builder.Property(c => c.FornecedorId)
               .IsRequired();

        //   builder.Ignore(c => c.ValorTotal);
        builder.Ignore(c => c.ValorTotal);

        builder.Metadata
            .FindNavigation(nameof(Compra.Itens))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany<ItemCompra>("_itens")
               .WithOne()
               .HasForeignKey(i => i.CompraId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Fornecedor>()
               .WithMany()
               .HasForeignKey(c => c.FornecedorId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}