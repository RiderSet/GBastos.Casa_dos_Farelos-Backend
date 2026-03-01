using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
using System.Collections.Concurrent;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Application.Mapping;

public sealed class IntegrationEventRegistry
{
    private readonly ConcurrentDictionary<Type, Func<IDomainEvent, IIntegrationEvent>> _mappings
        = new();

    public void Register<TDomainEvent>(
        Func<TDomainEvent, IIntegrationEvent> factory)
        where TDomainEvent : IDomainEvent
    {
        _mappings[typeof(TDomainEvent)] =
            (domainEvent) => factory((TDomainEvent)domainEvent);
    }

    public IIntegrationEvent? Map(IDomainEvent domainEvent)
    {
        return _mappings.TryGetValue(domainEvent.GetType(), out var factory)
            ? factory(domainEvent)
            : null;
    }
}