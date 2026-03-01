using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.IntegrationEvents;
using MassTransit;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Consumers;

public class PedidoCriadoConsumer
    : IConsumer<PedidoCriadoIntegrationEvent>
{
    private readonly PagamentoDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public PedidoCriadoConsumer(
        PagamentoDbContext context,
        IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<PedidoCriadoIntegrationEvent> context)
    {
        var message = context.Message;

        var pagamento = Pagamento.CriarPedido(
            pedidoId: message.PedidoId,
            valor: message.ValorTotal,
            clienteId: message.ClienteId
        );

        _context.Pagamentos.Add(pagamento);

        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(
            new PagamentoAprovadoIntegrationEvent(
                pagamento.Id,
                pagamento.PedidoId,
                pagamento.ClienteId,
                pagamento.ValorPG,
                pagamento.CriadoEmUtc
            ));
    }
}