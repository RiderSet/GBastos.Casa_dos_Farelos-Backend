namespace GBastos.Casa_dos_Farelos.PagamentoService.Application.Interfaces;

public interface IDistributedLockHandle : IAsyncDisposable
{
    bool IsAcquired { get; }
}