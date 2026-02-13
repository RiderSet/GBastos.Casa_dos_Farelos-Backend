using GBastos.Casa_dos_Farelos.Domain.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly List<Entity> _entitiesWithEvents = new();

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        var db = eventData.Context;

        if (db is null) return base.SavingChangesAsync(eventData, result, ct);

        var entities = db.ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.IntegrationEvent.Any())
            .Select(e => e.Entity)
            .ToList();

        _entitiesWithEvents.AddRange(entities);

        var outboxMessages = new List<OutboxMessage>();

        foreach (var entity in entities)
        {
            foreach (var domainEvent in entity.IntegrationEvent)
            {
                outboxMessages.Add(
                    OutboxMessage.Create(
                        domainEvent,
                        domainEvent.Id,
                        domainEvent.OccurredOn
                    )
                );
            }
        }

        db.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, ct);
    }
}
