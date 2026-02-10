using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Mappings
{
    public class ProdutoMapping : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).IsRequired().HasMaxLength(100);
            builder.Property(p => p.DescricaoProduto).IsRequired().HasMaxLength(500);
            builder.Property(p => p.PrecoVenda).HasPrecision(18, 2);
            builder.Property(p => p.QuantEstoque).IsRequired();

            builder.Property(p => p.CategoriaId).IsRequired();

            builder.HasOne(p => p.Categoria)
                   .WithMany(c => c.Produtos)
                   .HasForeignKey(p => p.CategoriaId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
