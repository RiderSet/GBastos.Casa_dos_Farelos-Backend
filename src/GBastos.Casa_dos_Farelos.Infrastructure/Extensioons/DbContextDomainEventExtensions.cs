using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Extensioons;

public static class DbContextDomainEventExtensions
{
    public static List<DomainEvent> GetDomainEvents(this DbContext context)
    {
        return context.ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(e => e.Entity.DomainEvents)
            .Cast<DomainEvent>()
            .ToList();
    }

    public static void ClearDomainEvents(this DbContext context)
    {
        var aggregates = context.ChangeTracker.Entries<AggregateRoot>();

        foreach (var aggregate in aggregates)
            aggregate.Entity.ClearDomainEvents();
    }
}
