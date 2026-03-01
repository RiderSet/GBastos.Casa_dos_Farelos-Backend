namespace GBastos.Casa_dos_Farelos.PagamentoService.Application.Interfaces;

public interface IDistributedLockFactory
{
    Task<IDistributedLockHandle> CreateLockAsync(
        string resource,
        TimeSpan expiry,
        TimeSpan wait,
        TimeSpan retry);
}
