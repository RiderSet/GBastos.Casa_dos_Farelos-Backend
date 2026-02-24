using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using RedLockNet;
using AppLockFactory =
    GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces.IDistributedLockFactory;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Locking;

public class RedisDistributedLockManager : IDistributedLockManager
{
    private readonly AppLockFactory _factory;

    public RedisDistributedLockManager(AppLockFactory factory)
    {
        _factory = factory;
    }

    public async Task<IDistributedLockHandle> AcquireLockAsync(
        string key,
        TimeSpan expiry)
    {
        var redLock = await _factory.CreateLockAsync(
            resource: key,
            expiry: expiry,
            wait: TimeSpan.FromSeconds(5),
            retry: TimeSpan.FromMilliseconds(200)
        );

        return redLock;
    }
}

internal class RedisLockHandle : IDistributedLockHandle
{
    private readonly IRedLock _redLock;

    public RedisLockHandle(IRedLock redLock)
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