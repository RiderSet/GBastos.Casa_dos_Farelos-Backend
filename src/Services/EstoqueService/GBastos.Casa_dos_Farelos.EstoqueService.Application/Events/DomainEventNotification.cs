using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Events;

public sealed class DomainEventNotification<T> : INotification
    where T : IDomainEvent
{
    public T DomainEvent { get; }

    public DomainEventNotification(T domainEvent)
    {
        DomainEvent = domainEvent;
    }
}