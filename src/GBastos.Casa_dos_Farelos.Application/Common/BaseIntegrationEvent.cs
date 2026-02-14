using GBastos.Casa_dos_Farelos.Application.Interfaces;

namespace GBastos.Casa_dos_Farelos.Application.Common;

public abstract record BaseIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}