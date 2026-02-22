using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.Contexts;
using GBastos.Casa_dos_Farelos.PagamentoService.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Outbox;

public sealed class OutboxDispatcher : IOutboxDispatcher
{
    private readonly PagamentoDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<OutboxDispatcher> _logger;

    public OutboxDispatcher(
        PagamentoDbContext context,
        IPublishEndpoint publishEndpoint,
        ILogger<OutboxDispatcher> logger)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task DispatchAsync(CancellationToken cancellationToken)
    {
        var messages = await _context.OutboxMessages
            .Where(x =>
                x.ProcessedOnUtc == null &&
                x.RetryCount < 5 &&
                (x.NextAttemptUtc == null ||
                 x.NextAttemptUtc <= DateTime.UtcNow))
            .OrderBy(x => x.OccurredOnUtc)
            .Take(20)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                var type = Type.GetType(message.Type);
                if (type is null)
                    continue;

                var integrationEvent =
                    JsonSerializer.Deserialize(message.Content, type);

                if (integrationEvent is null)
                    continue;

                await _publishEndpoint.Publish(integrationEvent, cancellationToken);

                message.MarkAsProcessed();
            }
            catch (Exception ex)
            {
                message.MarkAsFailed(ex.Message);

                var delaySeconds = Math.Pow(2, message.RetryCount);
                message.ScheduleNextAttempt(TimeSpan.FromSeconds(delaySeconds));
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        await Task.Delay(5000, cancellationToken);
    }
}