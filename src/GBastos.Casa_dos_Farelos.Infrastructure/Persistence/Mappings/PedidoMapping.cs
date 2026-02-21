using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Mappings;

public class PedidoMapping : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Data)
               .IsRequired();

        builder.Property(p => p.Total)
               .HasPrecision(18, 2);

        builder.Property(p => p.ClienteId)
               .IsRequired();

        // Relacionamento Cliente (TPH Pessoa)
        builder.HasOne(p => p.ClientePF)
               .WithMany()
               .HasForeignKey(p => p.ClienteId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.ClientePJ)
               .WithMany()
               .HasForeignKey(p => p.ClienteId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Itens)
               .WithOne(i => i.Pedido)
               .HasForeignKey(i => i.PedidoId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.Itens)
               .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}