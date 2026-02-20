using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Configuration;

public sealed class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
{
    public void Configure(EntityTypeBuilder<Pessoa> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Telefone)
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(p => p.Email)
               .HasMaxLength(150);
        builder
            .HasDiscriminator<string>("TipoPessoa")
            .HasValue<ClientePF>("PF")
            .HasValue<ClientePJ>("PJ");
    }
}