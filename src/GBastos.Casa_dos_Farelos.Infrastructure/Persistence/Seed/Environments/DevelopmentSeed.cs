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
            //new("João Silva", "11999999999", "2195959270", "glbsts@outlook.com", Convert.ToDateTime("03/30/1970")),
            //new("Maria Souza", "11888888888", "2197889270", "glbsts@outlook.com", Convert.ToDateTime("02/01/1974")),
            //new("Pedro Santos", "11777777777", "2197882021", "glbsts@outlook.com", Convert.ToDateTime("01/13/2006"))
            new("João Silva", "2195959270", "glbsts@outlook.com", "11999999999"),
            new("Beto Bozo", "2195959270", "glbsts@outlook.com", "22265152262"),
            new("Maria Pepino", "2195959270", "glbsts@outlook.com", "32132132321")
        };

        db.AddRange(clientes);
        await db.SaveChangesAsync(ct);
    }
}
