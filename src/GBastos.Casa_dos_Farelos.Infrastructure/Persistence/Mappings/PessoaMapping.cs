using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Mappings;

public class PessoaMapping : IEntityTypeConfiguration<Pessoa>
{
    public void Configure(EntityTypeBuilder<Pessoa> builder)
    {
        builder.ToTable("Pessoas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
               .HasMaxLength(150)
               .IsRequired();

        builder.Property(x => x.Documento)
               .HasMaxLength(20)
               .IsRequired();

        builder.HasDiscriminator<string>("TipoPessoa")
               .HasValue<ClientePF>("PF")
               .HasValue<ClientePJ>("PJ")
               .HasValue<Fornecedor>("FOR")
               .HasValue<Funcionario>("FUNC");
    }
}