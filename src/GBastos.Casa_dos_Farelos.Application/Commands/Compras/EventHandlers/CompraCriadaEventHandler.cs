using GBastos.Casa_dos_Farelos.Application.Dtos;
using GBastos.Casa_dos_Farelos.Application.IntegrationEvents;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Events.Compras;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Compras.EventHandlers;

public class CompraCriadaEventHandler : INotificationHandler<CompraCriadaDomainEvent>
{
    private readonly IOutbox _outbox;

    public CompraCriadaEventHandler(IOutbox outbox)
    {
        _outbox = outbox;
    }

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
            notification.TotalCompra,
            itensDto
        );

        Console.WriteLine($"[EventHandler] Compra {compraDto.CompraId} finalizada. Total: {compraDto.TotalCompra}");

        var integrationEvent = new CompraCriadaIntegrationEvent(
            notification.CompraId,
            notification.FornecedorId,
            notification.Total,
            itensDto
        );

        if (_outbox != null)
        {
            await _outbox.AddAsync(integrationEvent, cancellationToken);
        }
    }
}