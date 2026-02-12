using GBastos.Casa_dos_Farelos.Domain.Interfaces;

namespace GBastos.Casa_dos_Farelos.Domain.Common;

public abstract class DomainEvent : IDomainEvent
{
    public Guid Id { get; private set; }
    public DateTime OccurredOn { get; private set; }

    protected DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}