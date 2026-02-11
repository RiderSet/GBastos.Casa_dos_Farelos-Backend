using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Seed.Environments;

public static class StagingSeed
{
    public static Task Run(AppDbContext db, CancellationToken ct)
    {
        // dados mínimos para QA
        // sem dados pessoais reais

        return Task.CompletedTask;
    }
}