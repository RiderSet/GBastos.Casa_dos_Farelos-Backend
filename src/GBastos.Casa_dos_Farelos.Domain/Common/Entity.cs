using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Interfaces;

namespace GBastos.Casa_dos_Farelos.Domain.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    // private readonly List<IDomainEvent> _domainEvents = new();
    // public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private readonly List<IIntegrationEvent> _integrationEvent = new();
    public IReadOnlyCollection<IIntegrationEvent> IntegrationEvent => _integrationEvent;

    protected void AddDomainEvent(IIntegrationEvent integrationEvent)
        => _integrationEvent.Add(integrationEvent);

    public void ClearDomainEvents()
        => _integrationEvent.Clear();
}
