using GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Compras.Handlers;

public class CompraCriadaEventHandler : INotificationHandler<CompraCriadaDomainEvent>
{
    private readonly IIntegrationEventOutbox _outbox;

    public CompraCriadaEventHandler(IIntegrationEventOutbox outbox)
    {
        _outbox = outbox ?? throw new ArgumentNullException(nameof(outbox));
    }

    public async Task Handle(CompraCriadaDomainEvent notification, CancellationToken cancellationToken)
    {
        var itensDto = notification.Itens
            .Select(i => new ItemCompraDto
            {
                ProdutoId = i.ProdutoId,
                NomeProduto = i.NomeProduto,
                Quantidade = i.Quantidade,
                CustoUnitario = i.CustoUnitario,
                SubTotal = i.Quantidade * i.CustoUnitario
            })
            .ToList();

        // Calcula total da compra
        var valorTotal = itensDto.Sum(x => x.SubTotal);

        var compraDto = new CompraDto
        {
            Id = notification.CompraId,
            FuncionarioId = notification.FuncionarioId,
            ClienteId = notification.ClienteId,
            DataCompra = notification.DataCompra,
            Finalizada = true,
            Itens = itensDto
        };

        Console.WriteLine($"[EventHandler] Compra {compraDto.Id} finalizada. Total: {valorTotal}");

        // Cria integração para outbox
        var integrationEvent = new CompraCriadaIntegrationEvent(
            notification.CompraId,
            notification.FuncionarioId,
            valorTotal,
            itensDto
        );

        await _outbox.AddAsync(integrationEvent, cancellationToken);
    }

    // Método auxiliar para enviar eventos diretamente (opcional)
    public async Task HandleAsync(CompraCriadaIntegrationEvent evt, CancellationToken ct)
    {
        Console.WriteLine($"[Handler] Compra {evt.CompraId} finalizada. Total: {evt.ValorTotal}");
        await _outbox.AddAsync(evt, ct);
    }
}