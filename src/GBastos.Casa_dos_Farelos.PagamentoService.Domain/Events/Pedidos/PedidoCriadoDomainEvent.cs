using MediatR;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events;

public sealed record PedidoCriadoDomainEvent(
    Guid PedidoId,
    Guid ClienteId,
    decimal Total,
    DateTime OccurredOnUtc
) : INotification;