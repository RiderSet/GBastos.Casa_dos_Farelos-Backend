using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public OutboxWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var messages = await db.OutboxMessages
                .Where(x => x.ProcessedOnUtc == null)
                .OrderBy(x => x.OccurredOnUtc)
                .Take(50)
                .ToListAsync(stoppingToken);

            foreach (var msg in messages)
            {
                try
                {
                    var typeResolver = scope.ServiceProvider.GetRequiredService<IIntegrationEventTypeResolver>();

                    // despacha o evento
                    await IntegrationEventDispatcher.DispatchAsync(
                        scope.ServiceProvider,
                        typeResolver,
                        msg.Payload,
                        msg.EventName,
                        stoppingToken);

                    msg.MarkProcessed();
                }
                catch (Exception ex)
                {
                    msg.MarkFailed(ex.ToString());
                }
            }

            await db.SaveChangesAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromMilliseconds(500), stoppingToken);
        }
    }
}