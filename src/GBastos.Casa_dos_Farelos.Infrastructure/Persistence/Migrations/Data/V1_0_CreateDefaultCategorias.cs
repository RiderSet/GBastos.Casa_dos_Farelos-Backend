using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Migrations.Data;

public class V1_0_CreateDefaultCategorias : IDataMigration
{
    public string Id => "V1.0_CreateDefaultCategorias";
    public int Order => 1;

    public async Task Execute(AppDbContext db, CancellationToken ct)
    {
        if (db.Set<Categoria>().Any())
            return;

        db.AddRange(
            new Categoria("Rações", "Produtos alimentícios"),
            new Categoria("Acessórios", "Itens diversos"),
            new Categoria("Medicamentos", "Saúde animal")
        );

        await db.SaveChangesAsync(ct);
    }
}
