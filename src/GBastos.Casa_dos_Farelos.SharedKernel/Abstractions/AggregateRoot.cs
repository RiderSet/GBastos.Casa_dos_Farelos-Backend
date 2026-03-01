using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

public abstract class AggregateRoot<TId>
{
    public TId Id { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = new();

    protected AggregateRoot(TId id)
    {
        Id = id;
    }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();

    protected virtual void ValidateInvariants() { }
}