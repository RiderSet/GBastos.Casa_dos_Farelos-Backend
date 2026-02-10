using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox
{
    public sealed class OutboxProcessor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OutboxProcessor> _logger;

        public OutboxProcessor(
            IServiceScopeFactory scopeFactory,
            ILogger<OutboxProcessor> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox Processor iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessMessages(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro geral no OutboxProcessor");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task ProcessMessages(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var bus = scope.ServiceProvider.GetRequiredService<IEventBus>();

            var messages = await db.Outbox
                .Where(x => x.ProcessedOn == null)
                .OrderBy(x => x.OccurredOn)
                .Take(20)
                .ToListAsync(ct);

            if (messages.Count == 0)
                return;

            _logger.LogInformation("Processando {Count} mensagens do Outbox", messages.Count);

            foreach (var message in messages)
            {
                try
                {
                    var type = Type.GetType(message.Type);
                    if (type == null)
                    {
                        message.SetError($"Tipo não encontrado: {message.Type}");
                        continue;
                    }

                    var domainEvent = JsonSerializer.Deserialize(message.Payload, type);
                    if (domainEvent == null)
                    {
                        message.SetError("Falha ao desserializar evento");
                        continue;
                    }

                    await bus.Publish(domainEvent, ct);

                    message.MarkAsProcessed();
                }
                catch (Exception ex)
                {
                    message.SetError(ex.Message);
                    _logger.LogError(ex, "Erro ao processar mensagem {Id}", message.Id);
                }
            }

            await db.SaveChangesAsync(ct);
        }
    }
}