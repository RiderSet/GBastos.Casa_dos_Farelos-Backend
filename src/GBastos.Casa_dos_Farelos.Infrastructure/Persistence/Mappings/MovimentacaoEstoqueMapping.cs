using GBastos.Casa_dos_Farelos.Domain.Estoque;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Mappings;

public class MovimentacaoEstoqueMapping : IEntityTypeConfiguration<MovimentacaoEstoque>
{
    public void Configure(EntityTypeBuilder<MovimentacaoEstoque> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.NomeProduto)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(x => x.Tipo)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(x => x.Quantidade)
               .IsRequired();
    }
}