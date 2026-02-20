using GBastos.Casa_dos_Farelos.Domain.Interfaces;

namespace GBastos.Casa_dos_Farelos.Domain.Common;

/// <summary>
/// Entidade base para todas as entidades do domínio.
/// </summary>
public abstract class Entity
{
    public Guid Id { get; protected set; }

    private readonly List<IDomainEvent> _events = new();
    public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

    public void AddEvent(IDomainEvent @event) => _events.Add(@event);

    public void ClearEvents() => _events.Clear();

    /// <summary>
    /// Adiciona um evento de domínio à entidade.
    /// </summary>
    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _events.Add(domainEvent);
    }

    /// <summary>
    /// Limpa todos os eventos de domínio da entidade.
    /// </summary>
    public void ClearDomainEvents()
    {
        _events.Clear();
    }
}