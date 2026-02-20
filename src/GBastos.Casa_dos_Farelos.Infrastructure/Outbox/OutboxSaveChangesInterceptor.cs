using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Outbox;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
<<<<<<< HEAD
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
=======
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var domainEvents = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        if (domainEvents.Count == 0)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            var outboxMessage = OutboxMessage.CreateDomainEvent(domainEvent);
            dbContext.Set<OutboxMessage>().Add(outboxMessage);
        }

        // limpa eventos após salvar
        foreach (var entry in dbContext.ChangeTracker.Entries<AggregateRoot>())
            entry.Entity.ClearDomainEvents();
>>>>>>> 532a5516c5422679921d3b0f6d7a9995a5d30bda

       // dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}