using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Outbox;

public class OutboxInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var aggregates = context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .ToList();

        var outboxMessages = aggregates
            .SelectMany(e => e.Entity.DomainEvents)
            .Select(OutboxMessage.Create)
            .ToList();

        context.Set<OutboxMessage>().AddRange(outboxMessages);

        foreach (var aggregate in aggregates)
            aggregate.Entity.ClearDomainEvents();

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}