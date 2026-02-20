using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Seed.Environments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Seed.General;

public static class DatabaseSeeder
{
    public static async Task RunAsync(IServiceProvider services, CancellationToken ct)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        // seeds base (sempre executa)
        await ApplySeedAsync(db, "seed-admin", SeedAdmin, ct);
        await ApplySeedAsync(db, "seed-categorias", SeedCategorias, ct);

        // seeds por ambiente
        if (env.IsDevelopment())
            await ApplySeedAsync(db, "seed-dev", DevelopmentSeed.Run, ct);

        else if (env.IsStaging())
            await ApplySeedAsync(db, "seed-staging", StagingSeed.Run, ct);

        else if (env.IsProduction())
            await ApplySeedAsync(db, "seed-prod", ProductionSeed.Run, ct);
    }

    private static async Task ApplySeedAsync(
        AppDbContext db,
        string id,
        Func<AppDbContext, CancellationToken, Task> action,
        CancellationToken ct)
    {
        if (await db.DataSeedHistory.AnyAsync(x => x.Id == id, ct))
            return;

        await action(db, ct);

        db.DataSeedHistory.Add(new DataSeedHistory
        {
            Id = id,
            AppliedOnUtc = DateTime.UtcNow
        });

        await db.SaveChangesAsync(ct);
    }

    // ---------------- SEEDS ----------------

    private static async Task SeedAdmin(AppDbContext db, CancellationToken ct)
    {
        var admin = new Usuario(
            "admin@empresa.com",
            BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            "Admin"
        );

        db.Usuarios.Add(admin);
        await db.SaveChangesAsync(ct);
    }

    private static async Task SeedCategorias(AppDbContext db, CancellationToken ct)
    {
        if (await db.Set<Categoria>().AnyAsync(ct))
            return;

        db.AddRange(
            new Categoria("Rações", "Alimentos para animais"),
            new Categoria("Acessórios", "Itens diversos para pets"),
            new Categoria("Medicamentos", "Medicamentos veterinários")
        );

        await db.SaveChangesAsync(ct);
    }
}
