using GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;
using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events.Pagamentos;
using MassTransit;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Messaging.Consumers;

public class PagamentoAprovadoConsumer :
    IConsumer<PagamentoAprovadoEvent>
{
    private readonly IMediator _mediator;

    public PagamentoAprovadoConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(
        ConsumeContext<PagamentoAprovadoEvent> context)
    {
        var idempotencyKey = context.MessageId?.ToString()
            ?? Guid.NewGuid().ToString();

        await _mediator.Send(
            new ConfirmarReservaCommand(
                context.Message.PedidoId,
                idempotencyKey));
    }
}