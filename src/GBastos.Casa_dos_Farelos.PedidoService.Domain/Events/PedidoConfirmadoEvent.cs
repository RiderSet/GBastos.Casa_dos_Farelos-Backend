using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

public class PedidoConfirmadoEvent : IDomainEvent
{
    public Guid PedidoId { get; }
    public Guid ClienteId { get; }
    public decimal Total { get; }

    public PedidoConfirmadoEvent(
        Guid pedidoId,
        Guid clienteId,
        decimal total)
    {
        PedidoId = pedidoId;
        ClienteId = clienteId;
        Total = total;
    }
}