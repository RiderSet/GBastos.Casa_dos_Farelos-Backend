namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events;

public record PagamentoAprovadoEvent(
    Guid PagamentoId,
    Guid PedidoId
) : DomainEventPG;