using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using RedLockNet;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Locking;

public sealed class RedisDistributedLockHandle : IDistributedLockHandle
{
    private readonly IRedLock _redLock;

    public RedisDistributedLockHandle(IRedLock redLock)
    {
        _redLock = redLock;
    }

    public bool IsAcquired => _redLock.IsAcquired;

    public async ValueTask DisposeAsync()
    {
        _redLock.Dispose();
        await Task.CompletedTask;
    }
}