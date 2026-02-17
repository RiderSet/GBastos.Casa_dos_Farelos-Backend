using GBastos.Casa_dos_Farelos.Domain.Abstractions;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Pedidos.Handlers;

public sealed class PedidoCriadoDomainEventHandler
    : INotificationHandler<PedidoCriadoDomainEvent>
{
    private readonly IIntegrationEventOutbox _outbox;

    public PedidoCriadoDomainEventHandler(IIntegrationEventOutbox outbox)
        => _outbox = outbox;

    public async Task Handle(PedidoCriadoDomainEvent notification, CancellationToken ct)
    {
        var integrationEvent = new PedidoCriadoIntegrationEvent(
            Guid.NewGuid(),
            notification.PedidoId,
            notification.ClienteId,
            notification.Total,
            notification.OccurredOnUtc);

        await _outbox.AddAsync(integrationEvent, ct);
    }
}
