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
        var db = eventData.Context!;
        var messages = new List<OutboxMessage>();

        var entities = db.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.Events.Any())
            .Select(x => x.Entity);

        foreach (var entity in entities)
        {
            foreach (var domainEvent in entity.Events)
            {
                var message = new OutboxMessage(
                    domainEvent.GetType().FullName!,   // tipo completo é importante
                    JsonSerializer.Serialize(domainEvent)
                );

                messages.Add(message);
            }

            entity.ClearEvents();
        }

        if (messages.Count > 0)
            db.Set<OutboxMessage>().AddRange(messages);

        return base.SavingChangesAsync(eventData, result, ct);
    }
}
