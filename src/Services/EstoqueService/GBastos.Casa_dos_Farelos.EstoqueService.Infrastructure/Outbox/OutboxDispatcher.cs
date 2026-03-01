
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Interfaces;
using MassTransit;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;

public sealed class OutboxDispatcher
{
    private readonly IOutboxRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;

    public OutboxDispatcher(
        IOutboxRepository repository,
        IPublishEndpoint publishEndpoint)
    {
        _repository = repository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task DispatchAsync(CancellationToken cancellationToken)
    {
        var messages = await _repository.GetUnprocessedAsync(cancellationToken);

        foreach (var message in messages)
        {
            var type = Type.GetType(message.Type);

            if (type is null)
                continue;

            var integrationEvent =
                (IntegrationEvent?)JsonSerializer.Deserialize(
                    message.Content,
                    type);

            if (integrationEvent is null)
                continue;

            await _publishEndpoint.Publish(integrationEvent, cancellationToken);

            message.MarkAsProcessed();
        }

        await _repository.SaveChangesAsync(cancellationToken);
    }
}