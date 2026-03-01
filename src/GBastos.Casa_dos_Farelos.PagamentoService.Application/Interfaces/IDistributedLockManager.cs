namespace GBastos.Casa_dos_Farelos.PagamentoService.Application.Interfaces;

public interface IDistributedLockManager
{
    Task<IDistributedLockHandle> AcquireLockAsync(
        string key,
        TimeSpan expiry);
}