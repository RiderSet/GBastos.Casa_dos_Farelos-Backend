using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Domain.Events.IntegrationEvents;

public abstract class IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public DateTime OccurredOnUtc => OccurredOn;

    public virtual string EventType => GetType().Name;
    public virtual int Version => 1;
}