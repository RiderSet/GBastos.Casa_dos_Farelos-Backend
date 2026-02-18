using GBastos.Casa_dos_Farelos.Domain.Interfaces;

namespace GBastos.Casa_dos_Farelos.Domain.Events;

public abstract record DomainEvent : IDomainEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
}