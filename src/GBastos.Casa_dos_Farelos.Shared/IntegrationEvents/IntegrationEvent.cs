using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Domain.Abstractions;

public abstract record IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

    public string EventType => throw new NotImplementedException();

    public int Version => throw new NotImplementedException();
}
