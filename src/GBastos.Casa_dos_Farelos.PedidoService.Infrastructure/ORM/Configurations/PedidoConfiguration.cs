using GBastos.Casa_dos_Farelos.PedidoService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Entities;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.PedidoService.Infrastructure.ORM.Configurations;

public sealed class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.ClienteId)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion(
                status => status.Value,
                value => StatusPedido.From(value))
            .HasMaxLength(30)
            .IsRequired();

        builder.OwnsOne(p => p.Total, total =>
        {
            total.Property(m => m.Amount)
                .HasColumnName("TotalAmount")
                .HasPrecision(18, 2)
                .IsRequired();

            total.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.HasMany(typeof(ItemPedido), "_itens")
            .WithOne()
            .HasForeignKey("PedidoId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation("_itens")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(p => p.ClienteId);
        builder.HasIndex(p => p.Status);

        // ===========================
        // MULTI-TENANT (se existir)
        // ===========================
        // builder.HasIndex("TenantId");
    }
}