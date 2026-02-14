using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Domain.Common;

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