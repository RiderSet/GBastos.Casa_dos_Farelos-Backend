using GBastos.Casa_dos_Farelos.Domain.IntegrationsEvents;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IntegrationEventDispatcher _dispatcher;

    public OutboxWorker(IServiceScopeFactory scopeFactory, IntegrationEventDispatcher dispatcher)
    {
        _scopeFactory = scopeFactory;
        _dispatcher = dispatcher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var messages = await db.OutboxMessages
                .Where(x => x.ProcessedOn == null)
                .OrderBy(x => x.OccurredOn)
                .Take(50)
                .ToListAsync(stoppingToken);

            foreach (var msg in messages)
            {
                try
                {
                    await _dispatcher.DispatchAsync(
                        msg.EventName,
                        msg.Payload,
                        scope.ServiceProvider,
                        stoppingToken);

                    msg.MarkProcessed();
                }
                catch (Exception ex)
                {
                    msg.MarkFailed(ex.ToString());
                }
            }

            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(200, stoppingToken);
        }
    }
}
