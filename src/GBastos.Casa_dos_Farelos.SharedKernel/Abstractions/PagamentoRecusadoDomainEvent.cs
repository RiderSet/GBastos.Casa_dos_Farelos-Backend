using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;

public sealed class PagamentoRecusadoDomainEvent : IDomainEvent
{
    public Guid PagamentoId { get; }
    public Guid PedidoId { get; }
    public string Motivo { get; }
    public DateTime OccurredOnUtc { get; }

    public Guid EventId => Guid.NewGuid();

    public PagamentoRecusadoDomainEvent(
        Guid pagamentoId,
        Guid pedidoId,
        string motivo)
    {
        if (pagamentoId == Guid.Empty)
            throw new ArgumentException("PagamentoId inválido.");

        if (pedidoId == Guid.Empty)
            throw new ArgumentException("PedidoId inválido.");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("Motivo não pode ser vazio.");

        PagamentoId = pagamentoId;
        PedidoId = pedidoId;
        Motivo = motivo;
        OccurredOnUtc = DateTime.UtcNow;
    }
}