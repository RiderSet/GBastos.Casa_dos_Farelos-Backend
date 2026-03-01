using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;

public class OutboxBackgroundService : BackgroundService
{
    private readonly IServiceProvider _provider;

    public OutboxBackgroundService(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _provider.CreateScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<IOutboxDispatcher>();
            await dispatcher.DispatchAsync();
            await Task.Delay(5000, stoppingToken);
        }
    }
}