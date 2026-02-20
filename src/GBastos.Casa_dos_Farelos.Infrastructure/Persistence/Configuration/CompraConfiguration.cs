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

            builder.HasKey(c => c.Id);

            builder.Property(c => c.DataCompra)
                   .IsRequired();

            // RELAÇÃO CORRETA DO AGGREGATE
            builder.HasMany<ItemCompra>("_itens")
                   .WithOne(i => i.Compra)
                   .HasForeignKey(i => i.CompraId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Diz ao EF que a coleção pública não é mapeada diretamente
            builder.Ignore(c => c.Itens);
        }
    }
}