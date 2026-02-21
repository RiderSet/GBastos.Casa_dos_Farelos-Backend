using GBastos.Casa_dos_Farelos.Domain.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

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

        // Captura todos os aggregates com eventos
        var aggregates = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .ToList();

        if (!aggregates.Any())
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var outboxMessages = new List<OutboxMessage>();

        foreach (var aggregate in aggregates)
        {
            foreach (var domainEvent in aggregate.DomainEvents)
            {
                var outboxMessage = new OutboxMessage(
                    Guid.NewGuid(), // ID correto
                    domainEvent.GetType().FullName!,
                    JsonSerializer.Serialize(domainEvent),
                    DateTime.UtcNow
                );

                outboxMessages.Add(outboxMessage);
            }

            // Limpa eventos após capturar
            aggregate.ClearDomainEvents();
        }

        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}