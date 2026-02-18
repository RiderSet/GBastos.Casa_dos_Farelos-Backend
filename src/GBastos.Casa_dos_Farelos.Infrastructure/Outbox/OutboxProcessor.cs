using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxProcessor : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly IServiceProvider _provider;

    public OutboxProcessor(
        IServiceScopeFactory scopeFactory,
        IServiceProvider provider,
        ILogger<OutboxProcessor> logger)
    {
        _scopeFactory = scopeFactory;
        _provider = provider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _provider.CreateScope();

            var repo = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
            var resolver = scope.ServiceProvider.GetRequiredService<IIntegrationEventTypeResolver>();

            var messages = await repo.GetPendingAsync(20, stoppingToken);

            foreach (var msg in messages)
            {
                try
                {
                    await IntegrationEventDispatcher.DispatchAsync(
                        scope.ServiceProvider,
                        resolver,
                        msg.Payload,
                        msg.Type,
                        stoppingToken);

                    await repo.MarkAsProcessedAsync(msg.Id, stoppingToken);
                }
                catch (Exception ex)
                {
                    await repo.MarkAsFailedAsync(msg.Id, ex.Message, stoppingToken);
                }
            }

            await Task.Delay(1500, stoppingToken);
        }
    }

    private async Task ProcessMessages(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var bus = scope.ServiceProvider.GetRequiredService<IEventBus>();
        var resolver = scope.ServiceProvider.GetRequiredService<IIntegrationEventTypeResolver>();

        var messages = await db.OutboxMessages
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(50)
            .ToListAsync(ct);

        if (messages.Count == 0)
            return;

        _logger.LogInformation("Processando {Count} mensagens do Outbox", messages.Count);

        foreach (var message in messages)
        {
            try
            {
                var type = resolver.Resolve(message.EventName);
                if (type == null)
                {
                    message.MarkFailed($"Tipo não mapeado: {message.EventName}");
                    continue;
                }

                var integrationEvent = JsonSerializer.Deserialize(message.Payload, type);
                if (integrationEvent == null)
                {
                    message.MarkFailed("Falha ao desserializar evento");
                    continue;
                }

                await bus.Publish(integrationEvent, ct);
                message.MarkProcessed();
            }
            catch (Exception ex)
            {
                message.MarkFailed(ex.Message);
                _logger.LogError(ex, "Erro ao processar mensagem {Id}", message.Id);
            }
        }
        await db.SaveChangesAsync(ct);
    }
}