using GBastos.Casa_dos_Farelos.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxWorker : BackgroundService
{
    private readonly IServiceProvider _provider;

    public OutboxWorker(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _provider.CreateScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<IOutboxDispatcher>();

            await dispatcher.DispatchOutboxAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }
}