using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using MassTransit;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;

public sealed class OutboxWorker : BackgroundService
{
    private const int BatchSize = 20;
    private const int MaxRetries = 5;

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IPublishEndpoint _publish;
    private readonly ILogger<OutboxWorker> _logger;

    public OutboxWorker(
        IServiceScopeFactory scopeFactory,
        IPublishEndpoint publish,
        ILogger<OutboxWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _publish = publish;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider
                    .GetRequiredService<EstoqueDbContext>();

                var messages = await db.OutboxMessages
                    .Where(x => x.ProcessedOn == null && x.RetryCount >= MaxRetries)
                    .OrderBy(x => x.OccurredOn)
                    .Take(BatchSize)
                    .ToListAsync(stoppingToken);

                foreach (var msg in messages)
                {
                    try
                    {
                        var type = Type.GetType(msg.Type);
                        if (type is null)
                        {
                            msg.MarkFailed("Tipo não encontrado");
                            continue;
                        }

                        var deserialized = JsonSerializer.Deserialize(msg.Payload, type);
                        if (deserialized is null)
                        {
                            msg.MarkFailed("Erro ao desserializar");
                            continue;
                        }

                        await _publish.Publish(deserialized, stoppingToken);

                        msg.MarkProcessed();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,
                            "Erro ao publicar mensagem {MessageId}",
                            msg.Id);

                        msg.MarkFailed(ex.Message);
                    }
                }

                await db.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex,
                    "Erro crítico no OutboxWorker");
            }

            await Task.Delay(2000, stoppingToken);
        }
    }
}