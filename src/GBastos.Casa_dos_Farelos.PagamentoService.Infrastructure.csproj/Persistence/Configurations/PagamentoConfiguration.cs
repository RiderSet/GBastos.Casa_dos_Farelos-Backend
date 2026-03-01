using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.Configurations;

public class PagamentoConfiguration : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.IdempotencyKey)
            .IsUnique();

        builder.HasIndex(x => x.PedidoId);

        builder.Property(x => x.Moeda)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.MetodoPagamento)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.IdempotencyKey)
            .IsRequired()
            .HasMaxLength(100);
    }
}