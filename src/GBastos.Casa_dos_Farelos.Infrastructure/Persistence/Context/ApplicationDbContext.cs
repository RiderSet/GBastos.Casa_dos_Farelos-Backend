using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        ConfigureOutbox(modelBuilder);
    }

    private static void ConfigureOutbox(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OutboxMessage>(builder =>
        {
            builder.ToTable("outbox_messages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EventName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Payload)
                .IsRequired();

            builder.Property(x => x.OccurredOnUtc)
                .IsRequired();

            builder.Property(x => x.ProcessedOnUtc);

            builder.Property(x => x.Error)
                .HasMaxLength(4000);

            builder.HasIndex(x => x.ProcessedOnUtc);
            builder.HasIndex(x => x.OccurredOnUtc);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        await AddDomainEventsToOutboxAsync(ct);
        return await base.SaveChangesAsync(ct);
    }

    private async Task AddDomainEventsToOutboxAsync(CancellationToken ct)
    {
        var domainEntities = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var outboxMessages = new List<OutboxMessage>();

        foreach (var entry in domainEntities)
        {
            var events = entry.Entity.DomainEvents;

            foreach (var domainEvent in events)
            {
                var message = new OutboxMessage(
                    Guid.NewGuid(),
                    domainEvent.GetType().FullName!,
                    JsonSerializer.Serialize(domainEvent),
                    DateTime.UtcNow
                );

                outboxMessages.Add(message);
            }

            entry.Entity.ClearDomainEvents();
        }

        if (outboxMessages.Count > 0)
            await OutboxMessages.AddRangeAsync(outboxMessages, ct);
    }
}