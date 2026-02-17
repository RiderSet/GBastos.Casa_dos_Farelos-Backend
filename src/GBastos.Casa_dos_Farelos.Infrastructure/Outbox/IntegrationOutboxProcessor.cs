using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class IntegrationOutboxProcessor : BackgroundService
{
    private readonly IServiceProvider _provider;

    public IntegrationOutboxProcessor(IServiceProvider provider)
        => _provider = provider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await PublishPending();
            await Task.Delay(3000, stoppingToken);
        }
    }

    private async Task PublishPending()
    {
        using var scope = _provider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var bus = scope.ServiceProvider.GetRequiredService<IEventBus>();

        var messages = await db.Set<IntegrationOutboxMessage>()
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(50)
            .ToListAsync();

        foreach (var msg in messages)
        {
            try
            {
                var type = Type.GetType(msg.Type)!;
                var integrationEvent = (IIntegrationEvent)JsonSerializer.Deserialize(msg.Content, type)!;

                await bus.PublishAsync(integrationEvent, CancellationToken.None);

                msg.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                msg.Error = ex.Message;
            }
        }

        await db.SaveChangesAsync();
    }
}