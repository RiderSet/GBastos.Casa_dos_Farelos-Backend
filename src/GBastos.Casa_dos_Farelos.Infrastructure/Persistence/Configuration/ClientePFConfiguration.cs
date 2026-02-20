using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Configuration
{
    public sealed class ClientePFConfiguration : IEntityTypeConfiguration<ClientePF>
    {
        public void Configure(EntityTypeBuilder<ClientePF> builder)
        {
            builder.Property(x => x.CPF)
                .IsRequired()
                .HasMaxLength(11);

            builder.HasIndex(x => x.CPF)
                .IsUnique();

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Telefone)
                .HasMaxLength(20);

            builder.Property(x => x.Email)
                .HasMaxLength(150);

            // DataNascimento só se existir na entidade
            builder.Property(x => x.DtNascimento)
                .IsRequired();
        }
    }
}
