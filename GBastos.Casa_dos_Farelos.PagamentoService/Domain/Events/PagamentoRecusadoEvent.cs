namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events;

public record PagamentoRecusadoEvent(
    Guid PagamentoId,
    Guid PedidoId
) : DomainEventPG;