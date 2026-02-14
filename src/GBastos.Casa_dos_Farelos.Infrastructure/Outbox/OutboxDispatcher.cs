using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxDispatcher : IOutboxDispatcher
{
    private readonly AppDbContext _db;
    private readonly IPublisher _publisher;

    public OutboxDispatcher(AppDbContext db, IPublisher publisher)
    {
        _db = db;
        _publisher = publisher;
    }

    public async Task DispatchOutboxAsync(CancellationToken ct = default)
    {
        // pega somente eventos pendentes
        var messages = await _db.OutboxMessages
            .Where(x => !x.IsProcessed)
            .OrderBy(x => x.OccurredOn)
            .Take(50) // lote para não travar banco
            .ToListAsync(ct);

        if (messages.Count == 0)
            return;

        foreach (var message in messages)
        {
            try
            {
                // Descobre o tipo do evento
                var type = Type.GetType($"GBastos.Casa_dos_Farelos.Domain.Events.{message.Type}");

                if (type is null)
                {
                    message.MarkAsFailed("Tipo do evento não encontrado");
                    continue;
                }

                // Desserializa o evento
                var domainEvent = JsonSerializer.Deserialize(message.Payload, type);

                if (domainEvent is null)
                {
                    message.MarkAsFailed("Falha ao desserializar evento");
                    continue;
                }

                // Publica via MediatR (in-process)
                await _publisher.Publish(domainEvent, ct);

                // Marca como processado
                message.MarkAsProcessed();
            }
            catch (Exception ex)
            {
                message.MarkAsFailed(ex.Message);
            }
        }

        await _db.SaveChangesAsync(ct);
    }
}