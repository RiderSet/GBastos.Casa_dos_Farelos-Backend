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

            // resolve o resolver do DI uma vez
            var typeResolver = scope.ServiceProvider.GetRequiredService<IIntegrationEventTypeResolver>();

            // pega mensagens pendentes
            var messages = await db.OutboxMessages
                .Where(x => x.ProcessedOnUtc == null)
                .OrderBy(x => x.OccurredOnUtc)
                .Take(50)
                .ToListAsync(stoppingToken);

            foreach (var msg in messages)
            {
                try
                {
                    // despacha o evento automaticamente
                    await IntegrationEventDispatcher.DispatchAsync(
                        scope.ServiceProvider,
                        typeResolver,
                        msg.Payload,
                        msg.EventName,
                        stoppingToken);

                    msg.MarkAsProcessed();
                }
                catch (Exception ex)
                {
                    msg.MarkFailed(ex.ToString());
                }
            }

            // salva alterações no banco
            await db.SaveChangesAsync(stoppingToken);

            // pequena pausa antes da próxima verificação
            await Task.Delay(TimeSpan.FromMilliseconds(500), stoppingToken);
        }
    }
}