using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Configuration
{
    public class CompraConfiguration : IEntityTypeConfiguration<Compra>
    {
        public void Configure(EntityTypeBuilder<Compra> builder)
        {
            builder.ToTable("Compras");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FornecedorId).IsRequired();
            builder.Property(x => x.DataCompra).IsRequired();

            builder.Ignore(x => x.ValorTotal);

            builder.Metadata
                   .FindNavigation(nameof(Compra.Itens))!
                   .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany<ItemCompra>("_itens")
                   .WithOne()
                   .HasForeignKey(i => i.CompraId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}