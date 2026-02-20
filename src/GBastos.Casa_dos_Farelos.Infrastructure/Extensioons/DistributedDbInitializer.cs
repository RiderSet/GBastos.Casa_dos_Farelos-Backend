using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Extensioons;

public static class DistributedDbInitializer
{
    public static async Task EnsureMigratedAsync(IServiceProvider services, CancellationToken ct)
    {
        using var scope = services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var conn = (SqlConnection)db.Database.GetDbConnection();

        await conn.OpenAsync(ct);

        // 🔐 lock distribuído no banco
        using var cmd = new SqlCommand(
        """
        DECLARE @result int;
        EXEC @result = sp_getapplock
            @Resource = 'DB_MIGRATION_LOCK',
            @LockMode = 'Exclusive',
            @LockOwner = 'Session',
            @LockTimeout = 15000;

        SELECT @result;
        """, conn);

        var result = (int)(await cmd.ExecuteScalarAsync(ct) ?? -1);

        if (result < 0)
        {
            // outra instância já está migrando
            return;
        }

        try
        {
            await db.Database.MigrateAsync(ct);
        }
        finally
        {
            using var release = new SqlCommand(
                "EXEC sp_releaseapplock @Resource = 'DB_MIGRATION_LOCK', @LockOwner = 'Session';",
                conn);

            await release.ExecuteNonQueryAsync(ct);
        }
    }
}
