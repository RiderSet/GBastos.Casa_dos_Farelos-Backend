using Microsoft.Extensions.Configuration;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Locking;

public sealed class RedisDistributedLockFactory : IDistributedLockFactory
{
    private readonly RedLockFactory _factory;

    public RedisDistributedLockFactory(IConfiguration config)
    {
        var connectionString = config.GetConnectionString("Redis")
    ?? throw new InvalidOperationException(
        "Connection string 'Redis' não configurada.");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(
                "Connection string 'Redis' não configurada.");

        var redis = ConnectionMultiplexer.Connect(connectionString);

        _factory = RedLockFactory.Create(new List<RedLockMultiplexer>
    {
        redis
    });
    }

    public async Task<IDistributedLockHandle> CreateLockAsync(
        string resource,
        TimeSpan expiry,
        TimeSpan wait,
        TimeSpan retry)
    {
        var redLock = await _factory.CreateLockAsync(
            resource,
            expiry,
            wait,
            retry);

        return new RedisDistributedLockHandle(redLock);
    }
}