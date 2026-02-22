using MediatR;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }

    private readonly List<INotification> _domainEvents = new();
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents;

    protected void AddDomainEvent(INotification eventItem)
        => _domainEvents.Add(eventItem);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}