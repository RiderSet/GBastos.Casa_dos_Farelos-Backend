using GBastos.Casa_dos_Farelos.Domain.Events;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Domain.Common;

public sealed class DomainEventNotification<TDomainEvent> : INotification
    where TDomainEvent : DomainEvent
{
    public TDomainEvent DomainEvent { get; }

    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}