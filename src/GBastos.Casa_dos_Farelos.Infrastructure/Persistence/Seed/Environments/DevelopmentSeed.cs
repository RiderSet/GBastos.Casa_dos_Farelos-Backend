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
            new("João Silva", "11999999999", "2195959270", "glbsts@outlook.com", Convert.ToDateTime("13-01-2006")),
            new("Maria Souza", "11888888888", "2197889270", "glbsts@outlook.com", Convert.ToDateTime("01-02-2006")),
            new("Pedro Santos", "11777777777", "2197882021", "glbsts@outlook.com", Convert.ToDateTime("31-60-1998"))
        };

        db.AddRange(clientes);
        await db.SaveChangesAsync(ct);
    }
}
