using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

/// <summary>
/// Classe base de TODOS os Integration Events do sistema
/// </summary>
public abstract record BaseIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    public string EventType => GetType().Name;
    public int Version { get; init; } = 1;
}