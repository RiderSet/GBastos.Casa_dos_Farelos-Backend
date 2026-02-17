using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

public sealed record PedidoCriadoIntegrationEvent(
    Guid Id,
    Guid PedidoId,
    Guid ClienteId,
    decimal Total,
    DateTime OccurredOnUtc
) : IIntegrationEvent
{
    public string EventType => throw new NotImplementedException();

    public int Version => throw new NotImplementedException();
}
