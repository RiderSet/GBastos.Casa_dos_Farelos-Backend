using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Seed.Environments;

public static class ProductionSeed
{
    public static async Task Run(AppDbContext db, CancellationToken ct)
    {
        // produção NÃO recebe dados fake
        await Task.CompletedTask;
    }
}