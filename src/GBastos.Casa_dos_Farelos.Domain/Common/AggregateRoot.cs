using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBastos.Casa_dos_Farelos.Domain.Common;

public abstract class AggregateRoot : IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents
        => new ReadOnlyCollection<IDomainEvent>(_domainEvents);

    protected void Raise(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent is null)
            throw new ArgumentNullException(nameof(domainEvent));

        _domainEvents.Add(domainEvent);
    }

    internal void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent is null) return;

        _domainEvents.Remove(domainEvent);
    }

    public void Validate()
    {
        ValidateInvariants();
    }

    public void ClearDomainEvents()
        => _domainEvents.Clear();

    protected abstract void ValidateInvariants();
}