using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Mappings;

public class ItemVendaMapping : IEntityTypeConfiguration<ItemVenda>
{
    public void Configure(EntityTypeBuilder<ItemVenda> builder)
    {
        builder.ToTable("ItensVenda");

        builder.HasKey(i => i.Id);

        // ================= CAMPOS =================
        builder.Property(i => i.ProdutoId)
               .IsRequired();

        builder.Property(i => i.VendaId)
               .IsRequired();

        builder.Property(i => i.Quantidade)
               .IsRequired();

        builder.Property(i => i.PrecoUnitario)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(i => i.SubTotal)
               .HasPrecision(18, 2)
               .IsRequired();

        builder.Property(i => i.DescricaoProduto)
               .HasMaxLength(200)
               .IsRequired();

        // ================= RELACIONAMENTO =================

        // Produto (somente referência)
        builder.HasOne(i => i.Produto)
               .WithMany()
               .HasForeignKey(i => i.ProdutoId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}