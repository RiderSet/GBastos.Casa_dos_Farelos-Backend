using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Outbox;

public class OutboxBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public OutboxBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider
                .GetRequiredService<CadastroDbContext>();

            var messages = await context.OutboxMessages
                .Where(x => x.ProcessedOnUtc == null)
                .OrderBy(x => x.OccurredOnUtc)
                .Take(20)
                .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                var type = Type.GetType(message.Type);
                var @event = JsonSerializer.Deserialize(
                    message.Content,
                    type!);

                // Para publicar no broker
                // Exemplo: await _publisher.Publish(@event);

                message.MarkAsProcessed();
            }

            await context.SaveChangesAsync(stoppingToken);
            await Task.Delay(5000, stoppingToken);
        }
    }
}