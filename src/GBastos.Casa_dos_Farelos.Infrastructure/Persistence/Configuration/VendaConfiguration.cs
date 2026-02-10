using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Configuration;

public class VendaConfiguration : IEntityTypeConfiguration<Venda>
{
    public void Configure(EntityTypeBuilder<Venda> builder)
    {
        builder.ToTable("Vendas");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.DataVenda)
               .IsRequired();

        builder.Property(v => v.ClienteId)
               .IsRequired();

        builder.Property(v => v.FuncionarioId)
               .IsRequired();

        // BACKING FIELD
        builder.Metadata
            .FindNavigation(nameof(Venda.Itens))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany<ItemVenda>("_itens")
               .WithOne(i => i.Venda)
               .HasForeignKey(i => i.VendaId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
