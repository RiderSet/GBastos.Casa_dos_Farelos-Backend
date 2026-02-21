namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events;

public record PagamentoCriadoEvent(
    Guid PagamentoId,
    Guid PedidoId,
    decimal Valor
) : DomainEventPG;