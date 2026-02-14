using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var domainEvents = context.ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        if (domainEvents.Count == 0)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            var outboxMessage = OutboxMessage.Create(
                domainEvent,
                Guid.NewGuid(),
                DateTime.UtcNow);

            context.Set<OutboxMessage>().Add(outboxMessage);
        }

        // limpa eventos após salvar
        foreach (var entry in context.ChangeTracker.Entries<AggregateRoot>())
            entry.Entity.ClearDomainEvents();

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}