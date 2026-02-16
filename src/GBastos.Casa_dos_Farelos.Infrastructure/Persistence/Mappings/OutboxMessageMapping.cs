using GBastos.Casa_dos_Farelos.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Mappings;

public sealed class OutboxMessageMapping : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("outbox_messages");

        builder.HasKey(x => x.Id);

        // routing key do evento
        builder.Property(x => x.EventName)
            .HasMaxLength(200)
            .IsRequired();

        // json do evento
        builder.Property(x => x.Payload)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        // quando ocorreu
        builder.Property(x => x.OccurredOn)
            .IsRequired();

        // controle de processamento
        builder.Property(x => x.ProcessedOn)
            .IsRequired(false);

        builder.Property(x => x.Error)
            .HasColumnType("nvarchar(max)")
            .IsRequired(false);

        builder.Ignore(x => x.IsProcessed);

        // 🔥 índice crítico do outbox (performance)
        builder.HasIndex(x => new { x.ProcessedOn, x.OccurredOn })
            .HasDatabaseName("IX_Outbox_Processing");
    }
}