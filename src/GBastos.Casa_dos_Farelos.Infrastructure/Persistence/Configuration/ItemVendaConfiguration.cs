using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Configuration
{
    public class ItemVendaConfiguration : IEntityTypeConfiguration<ItemVenda>
    {
        public void Configure(EntityTypeBuilder<ItemVenda> builder)
        {
            builder.ToTable("ItensVenda");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Quantidade)
                   .IsRequired();

            builder.Property(i => i.PrecoUnitario)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.Property(i => i.SubTotal)
                   .HasPrecision(18, 2)
                   .IsRequired();

            builder.HasOne(i => i.Produto)
                   .WithMany()
                   .HasForeignKey(i => i.ProdutoId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
