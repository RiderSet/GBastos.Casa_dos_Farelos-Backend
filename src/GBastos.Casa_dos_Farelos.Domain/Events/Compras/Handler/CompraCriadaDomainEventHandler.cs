using GBastos.Casa_dos_Farelos.Application.Commands.Compras.IntegrationsEvents;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Compras.Handler;

public class CompraCriadaDomainEventHandler
    : INotificationHandler<CompraCriadaDomainEvent>
{
    private readonly IOutbox _outbox;

    public CompraCriadaDomainEventHandler(IOutbox outbox)
    {
        _outbox = outbox;
    }

    public async Task Handle(CompraCriadaDomainEvent notification, CancellationToken ct)
    {
        var integration = new CompraCriadaIntegrationEvent(
            notification.CompraId,
            notification.FornecedorId,
            notification.ValorTotal,
            notification.Itens
        );

        await _outbox.AddAsync(integration);
    }
}