namespace GBastos.Casa_dos_Farelos.SharedKernel.Persistence;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}