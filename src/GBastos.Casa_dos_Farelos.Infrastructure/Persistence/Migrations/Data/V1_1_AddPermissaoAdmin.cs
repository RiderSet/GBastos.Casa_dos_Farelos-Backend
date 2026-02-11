using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Migrations.Data;

public class V1_1_AddPermissaoAdmin : IDataMigration
{
    public string Id => "V1.1_AddPermissaoAdmin";
    public int Order => 2;

    public async Task Execute(AppDbContext db, CancellationToken ct)
    {
        var admin = await db.Usuarios.FirstAsync(ct);

        admin.DefinirRole("Admin"); // dispara Domain Event

        await db.SaveChangesAsync(ct);
    }
}