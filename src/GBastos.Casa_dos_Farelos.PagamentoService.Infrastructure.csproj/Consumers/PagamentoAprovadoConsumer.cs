using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Entities;
using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.IntegrationEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Interfaces;

public sealed class PagamentoAprovadoConsumer :
    IConsumer<PagamentoAprovadoIntegrationEvent>
{
    private readonly PagamentoDbContext _context;

    public PagamentoAprovadoConsumer(PagamentoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
    PagamentoAprovadoIntegrationEvent integrationEvent,
    CancellationToken cancellationToken)
    {
        var message = OutboxMessagePG.Create(integrationEvent);
        await _context.OutboxMessages.AddAsync(message, cancellationToken);
    }

    public async Task Consume(
        ConsumeContext<PagamentoAprovadoIntegrationEvent> context)
    {
        var eventId = context.Message.Id.ToString();

        var alreadyProcessed = await _context.InboxMessages
            .AnyAsync(x => x.EventId == eventId, context.CancellationToken);

        if (alreadyProcessed)
            return;

        var pedido = await _context.Pedidos
            .FirstOrDefaultAsync(
                x => x.Id == context.Message.PedidoId,
                context.CancellationToken);

        if (pedido is null)
            return; // ou logar

        pedido.MarcarComoPago();

        _context.InboxMessages.Add(new InboxMessage
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            ReceivedOnUtc = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(context.CancellationToken);
    }
}