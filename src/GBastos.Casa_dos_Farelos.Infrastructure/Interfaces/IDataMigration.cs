using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;

public interface IDataMigration
{
    string Id { get; }          // versão única
    int Order { get; }          // ordem de execução
    Task Execute(AppDbContext db, CancellationToken ct);
}
