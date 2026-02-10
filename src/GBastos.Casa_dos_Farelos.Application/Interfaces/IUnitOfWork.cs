namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken ct = default);
}
