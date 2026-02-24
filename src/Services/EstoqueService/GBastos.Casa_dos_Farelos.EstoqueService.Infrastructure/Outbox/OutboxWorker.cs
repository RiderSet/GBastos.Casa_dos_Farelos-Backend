using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Persistence.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;

public sealed class OutboxWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IPublishEndpoint _publish;

    public OutboxWorker(
        IServiceScopeFactory scopeFactory,
        IPublishEndpoint publish)
    {
        _scopeFactory = scopeFactory;
        _publish = publish;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EstoqueDbContext>();

            var messages = await db.OutboxMessages
                .Where(x => x.ProcessedOn == null)
                .Take(20)
                .ToListAsync(stoppingToken);

            foreach (var msg in messages)
            {
                var type = Type.GetType(msg.Type);

                if (type is null)
                    continue;

                var deserialized = JsonSerializer.Deserialize(
                    msg.Payload,
                    type);

                if (deserialized is null)
                    continue;

                await _publish.Publish(deserialized, stoppingToken);

                msg.MarkProcessed();
            }

            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(2000, stoppingToken);
        }
    }
}