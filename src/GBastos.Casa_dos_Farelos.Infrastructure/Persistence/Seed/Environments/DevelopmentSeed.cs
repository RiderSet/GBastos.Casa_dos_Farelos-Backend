using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Seed.Environments;

public static class DevelopmentSeed
{
    public static async Task Run(AppDbContext db, CancellationToken ct)
    {
        if (await db.Set<ClientePF>().AnyAsync(ct))
            return;

        var clientes = new List<ClientePF>
{
    ClientePF.CriarClientePF(
        "João Silva",
        "11999999999",
        "glbsts@outlook.com",
        "12345678909",
        new DateTime(1970, 3, 30)),

    ClientePF.CriarClientePF(
        "Maria Souza",
        "11888888888",
        "maria@email.com",
        "11144477735",
        new DateTime(1974, 2, 1)),

    ClientePF.CriarClientePF(
        "Pedro Santos",
        "11777777777",
        "pedro@email.com",
        "52998224725",
        new DateTime(2006, 1, 13))
};

        db.AddRange(clientes);
        await db.SaveChangesAsync(ct);
    }
}
