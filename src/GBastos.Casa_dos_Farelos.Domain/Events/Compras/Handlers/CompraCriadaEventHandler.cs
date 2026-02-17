using GBastos.Casa_dos_Farelos.Domain.Dtos;
using GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;
using GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Compras.Handlers;

public class CompraCriadaEventHandler : INotificationHandler<CompraCriadaDomainEvent>
{
    private readonly IIntegrationEventOutbox _outbox;

    public CompraCriadaEventHandler(IIntegrationEventOutbox outbox) => _outbox = outbox;

    public async Task Handle(CompraCriadaDomainEvent notification, CancellationToken cancellationToken)
    {
        var itensDto = notification.Itens
            .Select(i => new CompraItemDto(
                i.ProdutoId,
                i.NomeProduto,
                i.Quantidade,
                i.CustoUnitario,
                i.SubTotal
            ))
            .ToList();

        var compraDto = new CompraDto(
            notification.CompraId,
            notification.FornecedorId,
            notification.ValorTotal,
            itensDto
        );

        Console.WriteLine($"[EventHandler] Compra {compraDto.CompraId} finalizada. Total: {compraDto.ValorTotal}");

        var integrationEvent = new CompraCriadaIntegrationEvent(
            notification.CompraId,
            notification.FornecedorId,
            notification.ValorTotal,
            itensDto
        );

        if (_outbox != null)
        {
            await _outbox.AddAsync(integrationEvent, cancellationToken);
        }
    }

    public async Task HandleAsync(CompraCriadaIntegrationEvent evt, CancellationToken ct)
    {
        Console.WriteLine($"[Handler] Compra {evt.CompraId} finalizada. Total: {evt.ValorTotal}");

        if (_outbox != null)
        {
            await _outbox.AddAsync(evt, ct);
        }
    }
}