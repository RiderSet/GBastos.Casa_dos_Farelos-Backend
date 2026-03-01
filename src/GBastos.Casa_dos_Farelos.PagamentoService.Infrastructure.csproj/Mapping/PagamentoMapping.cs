using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Mapping;

public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ValorPG).HasPrecision(18, 2);
        builder.Property(x => x.Status).HasConversion<int>();
    }
}