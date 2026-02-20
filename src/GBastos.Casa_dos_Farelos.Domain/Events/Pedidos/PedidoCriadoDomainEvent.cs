using GBastos.Casa_dos_Farelos.Domain.Interfaces;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Pedidos;

public sealed record PedidoCriadoDomainEvent(
    Guid PedidoId,
    Guid ClienteId,
    decimal Total
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}