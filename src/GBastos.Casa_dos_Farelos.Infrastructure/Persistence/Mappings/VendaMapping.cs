using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Mappings;

public class VendaMapping : IEntityTypeConfiguration<Venda>
{
    public void Configure(EntityTypeBuilder<Venda> builder)
    {
        builder.ToTable("Vendas");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.DataVenda)
               .IsRequired();

        builder.Property(v => v.TotalVenda)
               .HasPrecision(18, 2);

        builder.Property(v => v.ClienteId)
               .IsRequired();

        builder.Property(v => v.FuncionarioId)
               .IsRequired();

        // -------- RELACIONAMENTOS --------

        builder.HasOne<ClientePF>()
               .WithMany()
               .HasForeignKey(v => v.ClienteId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<ClientePJ>()
               .WithMany()
               .HasForeignKey(v => v.ClienteId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Funcionario>()
               .WithMany()
               .HasForeignKey(v => v.FuncionarioId)
               .OnDelete(DeleteBehavior.Restrict);

        // -------- BACKING FIELD (ESSENCIAL) --------

        builder.Metadata
            .FindNavigation(nameof(Venda.Itens))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany<ItemVenda>("_itens")
               .WithOne(i => i.Venda)
               .HasForeignKey(i => i.VendaId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
