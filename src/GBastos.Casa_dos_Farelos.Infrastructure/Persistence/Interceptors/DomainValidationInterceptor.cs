using GBastos.Casa_dos_Farelos.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Interceptors;

public sealed class DomainValidationInterceptor : SaveChangesInterceptor
{
    private static void ValidateAggregates(DbContext? context)
    {
        if (context is null) return;

        var aggregates = context.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e =>
                e.State == EntityState.Added ||
                e.State == EntityState.Modified)
            .Select(e => e.Entity)
            .ToList();

        foreach (var aggregate in aggregates)
        {
            aggregate.Validate();
        }
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ValidateAggregates(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ValidateAggregates(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}