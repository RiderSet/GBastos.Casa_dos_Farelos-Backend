using GBastos.Casa_dos_Farelos.Domain.Abstractions;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Domain.Events;

public abstract class DomainEvent : IDomainEvent, INotification
{
    public Guid Id { get; }
    public DateTime OccurredOn { get; }

    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}