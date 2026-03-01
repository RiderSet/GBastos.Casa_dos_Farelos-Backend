using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Common;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events.Pagamentos;

public record PagamentoRecusadoEvent(
    Guid PagamentoId,
    Guid PedidoId,
    string Motivo
) : DomainEventPG;