using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Common;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events;

public sealed record PagamentoCriadoEvent(
    Guid PagamentoId,
    Guid PedidoId,
    decimal Valor
) : DomainEventPG;