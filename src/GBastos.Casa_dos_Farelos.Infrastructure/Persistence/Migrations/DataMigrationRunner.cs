using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Seed.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Migrations;

public static class DataMigrationRunner
{
    public static async Task RunAsync(IServiceProvider services, CancellationToken ct)
    {
        using var scope = services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var migrations = scope.ServiceProvider.GetServices<IDataMigration>()
            .OrderBy(x => x.Order)
            .ToList();

        await db.Database.MigrateAsync(ct);

        foreach (var migration in migrations)
        {
            var exists = await db.Set<DataMigrationHistory>()
                .AnyAsync(x => x.Id == migration.Id, ct);

            if (exists)
                continue;

            await migration.Execute(db, ct);

            db.Add(new DataMigrationHistory
            {
                Id = migration.Id,
                AppliedOnUtc = DateTime.UtcNow
            });

            await db.SaveChangesAsync(ct);
        }
    }
}