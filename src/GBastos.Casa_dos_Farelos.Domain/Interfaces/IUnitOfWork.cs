namespace GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
