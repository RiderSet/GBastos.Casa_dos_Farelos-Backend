using GBastos.Casa_dos_Farelos.Application.Interfaces;

namespace GBastos.Casa_dos_Farelos.Domain.IntegrationsEvents;

public abstract record IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}