using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxDispatcher : IOutboxDispatcher
{
    private readonly AppDbContext _db;
    private readonly IPublisher _publisher;
    private readonly IIntegrationEventTypeResolver _resolver;

    public OutboxDispatcher(AppDbContext db, IPublisher publisher, IIntegrationEventTypeResolver resolver)
    {
        _db = db;
        _publisher = publisher;
        _resolver = resolver;
    }

    public async Task DispatchOutboxAsync(CancellationToken ct = default)
    {
        var messages = await _db.OutboxMessages
            .Where(x => !x.IsProcessed)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(50)
            .ToListAsync(ct);

        if (messages.Count == 0)
            return;

        foreach (var message in messages)
        {
            try
            {
                var type = _resolver.Resolve(message.EventName);

                if (type is null)
                {
                    message.MarkFailed($"Tipo não encontrado: {message.EventName}");
                    continue;
                }

                var domainEvent = JsonSerializer.Deserialize(message.Payload, type);

                if (domainEvent is null)
                {
                    message.MarkFailed("Falha ao desserializar evento");
                    continue;
                }

                await _publisher.Publish(domainEvent, ct);

                message.MarkProcessed();
            }
            catch (Exception ex)
            {
                message.MarkFailed(ex.Message);
            }
        }

        await _db.SaveChangesAsync(ct);
    }
}