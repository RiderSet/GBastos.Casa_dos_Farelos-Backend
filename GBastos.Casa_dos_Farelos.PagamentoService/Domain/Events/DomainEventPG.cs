using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Interfaces;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events;

public abstract record DomainEventPG : IDomainEvent
{
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
}