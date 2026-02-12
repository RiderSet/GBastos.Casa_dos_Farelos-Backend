using GBastos.Casa_dos_Farelos.Domain.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        var db = eventData.Context;
        if (db is null) return base.SavingChangesAsync(eventData, result, ct);

        var entities = db.ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .ToList();

        var outboxMessages = new List<OutboxMessage>();

        foreach (var entry in entities)
        {
            foreach (var domainEvent in entry.Entity.DomainEvents)
            {
                outboxMessages.Add(
                    OutboxMessage.Create(
                        domainEvent,
                        domainEvent.Id,
                        domainEvent.OccurredOn
                    )
                );
            }

            entry.Entity.ClearDomainEvents();
        }

        db.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, ct);
    }
}
