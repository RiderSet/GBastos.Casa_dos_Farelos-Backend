namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public interface IDistributedLockHandle : IAsyncDisposable
{
    bool IsAcquired { get; }
}