using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Domain.Events;

public class PedidoCanceladoEvent : IDomainEvent
{
    public Guid PedidoId { get; }
    public Guid ClienteId { get; }

    public PedidoCanceladoEvent(Guid pedidoId, Guid clienteId)
    {
        PedidoId = pedidoId;
        ClienteId = clienteId;
    }
}