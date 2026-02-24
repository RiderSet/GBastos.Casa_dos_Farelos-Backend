namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public interface IDistributedLockManager
{
    Task<IDistributedLockHandle> AcquireLockAsync(
        string key,
        TimeSpan expiry);
}