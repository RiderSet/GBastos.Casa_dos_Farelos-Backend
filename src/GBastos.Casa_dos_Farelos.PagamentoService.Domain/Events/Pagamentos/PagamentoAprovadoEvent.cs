using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Common;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events.Pagamentos;

public record PagamentoAprovadoEvent(
    Guid PagamentoId,
    Guid PedidoId,
    Guid ClienteId,
    decimal ValorPg
) : DomainEventPG;