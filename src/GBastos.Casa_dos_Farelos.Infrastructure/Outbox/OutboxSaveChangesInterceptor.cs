using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Outbox;
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
        var dbContext = eventData.Context;

        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, ct);

        var domainEntities = dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Events.Any())
            .Select(x => x.Entity)
            .ToList();

        if (domainEntities.Count == 0)
            return base.SavingChangesAsync(eventData, result, ct);

        var outboxMessages = new List<OutboxMessage>();

        foreach (var entity in domainEntities)
        {
            foreach (var domainEvent in entity.Events)
            {
                var message = new OutboxMessage(
                    domainEvent.GetType().FullName!,
                    JsonSerializer.Serialize(domainEvent)
                );

                outboxMessages.Add(message);
            }
            entity.ClearDomainEvents();
        }

        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, ct);
    }
}
