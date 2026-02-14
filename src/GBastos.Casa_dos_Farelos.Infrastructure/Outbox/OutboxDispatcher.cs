using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxDispatcher : BackgroundService
{
    private readonly IServiceProvider _provider;

    public OutboxDispatcher(IServiceProvider provider)
        => _provider = provider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var messages = await db.OutboxMessages
                .Where(x => x.ProcessedOn == null)
                .OrderBy(x => x.OccurredOn)
                .Take(50)
                .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                try
                {
                    var type = Type.GetType(message.Type)!;
                    var @event = JsonSerializer.Deserialize(message.Payload, type)!;

                    await publisher.Publish(@event, stoppingToken);

                    message.MarkAsProcessed();
                }
                catch (Exception ex)
                {
                    message.MarkAsFailed(ex.Message);
                }
            }

            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(2000, stoppingToken);
        }
    }
}