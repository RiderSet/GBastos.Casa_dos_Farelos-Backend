using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Outbox;

public class OutboxProcessor : BackgroundService
{
    private readonly IServiceProvider _sp;

    public OutboxProcessor(IServiceProvider sp)
    {
        _sp = sp;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _sp.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<CadastroDbContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<RabbitMqPublisher>();

            var messages = await db.OutboxMessages
                .Where(x => x.ProcessedOnUtc == null)
                .OrderBy(x => x.OccurredOnUtc)
                .Take(20)
                .ToListAsync(stoppingToken);

            foreach (var msg in messages)
            {
                try
                {
                    await publisher.PublishAsync(
                        msg.Type,
                        msg.Payload,
                        stoppingToken);

                    msg.MarkAsProcessed();
                }
                catch (Exception ex)
                {
                    msg.MarkFailed(ex.Message);
                }
            }

            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(2000, stoppingToken);
        }
    }
}