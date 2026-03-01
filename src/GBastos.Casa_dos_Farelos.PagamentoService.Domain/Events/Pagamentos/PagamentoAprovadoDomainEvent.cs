using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events.Pagamentos;

public sealed class PagamentoAprovadoDomainEvent : IDomainEvent
{
    public Guid PagamentoId { get; }
    public Guid PedidoId { get; }
    public decimal Valor { get; }
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;

    public Guid EventId => Guid.NewGuid();

    public PagamentoAprovadoDomainEvent(Guid pagamentoId, Guid pedidoId, decimal valor)
    {
        PagamentoId = pagamentoId;
        PedidoId = pedidoId;
        Valor = valor;
    }
}