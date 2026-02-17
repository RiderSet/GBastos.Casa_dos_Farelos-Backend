using GBastos.Casa_dos_Farelos.Domain.Common;
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

       // dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}