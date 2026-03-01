using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Persistence.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Telefone)
            .HasMaxLength(20);

        builder.HasDiscriminator<string>("TipoCliente")
            .HasValue<ClientePF>("PF")
            .HasValue<ClientePJ>("PJ");

        builder.OwnsOne(typeof(Cpf), "Cpf", cpf =>
        {
            cpf.Property<string>("Numero")
                .HasColumnName("Cpf")
                .HasMaxLength(11);
        });

        builder.OwnsOne(typeof(Cnpj), "Cnpj", cnpj =>
        {
            cnpj.Property<string>("Numero")
                .HasColumnName("Cnpj")
                .HasMaxLength(14);
        });
    }
}