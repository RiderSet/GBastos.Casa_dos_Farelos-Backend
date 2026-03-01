using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events.Pagamentos;
using GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.IntegrationEvents;
using MediatR;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Application.Handlers;

public sealed class PagamentoAprovadoDomainEventHandler
    : INotificationHandler<PagamentoAprovadoEvent>
{
    private readonly IIntegrationEventOutbox _outbox;

    public PagamentoAprovadoDomainEventHandler(
        IIntegrationEventOutbox outbox)
    {
        _outbox = outbox;
    }

    public async Task Handle(
        PagamentoAprovadoEvent notification,
        CancellationToken cancellationToken)
    {
        var integrationEvent =
            new PagamentoAprovadoIntegrationEvent(
                Guid.NewGuid(),
                notification.PedidoId,
                notification.ClienteId,
                notification.ValorPg,
                notification.OccurredOnUtc
            );

        await _outbox.AddAsync(integrationEvent, cancellationToken);
    }
}