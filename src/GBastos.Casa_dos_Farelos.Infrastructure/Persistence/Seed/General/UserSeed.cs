using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Seed.General;

public static class UserSeed
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (db.Usuarios.Any())
            return;

        var admin = new Usuario(
            login: "admin",
            senha: "123456",
            perfil: "Gerente");

        db.Usuarios.Add(admin);
        await db.SaveChangesAsync();
    }
}