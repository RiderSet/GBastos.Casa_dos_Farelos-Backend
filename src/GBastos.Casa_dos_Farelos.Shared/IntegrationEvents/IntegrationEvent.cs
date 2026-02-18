using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

/// <summary>
/// Classe base para todos os eventos de integração (IntegrationEvent)
/// </summary>
public abstract class IntegrationEvent : IIntegrationEvent
{
    /// <summary>
    /// Identificador único do evento
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Momento em que o evento ocorreu (UTC)
    /// </summary>
    public DateTime OccurredOnUtc { get; }

    /// <summary>
    /// Nome do tipo do evento
    /// </summary>
    public string EventType => GetType().Name;

    /// <summary>
    /// Versão do evento (para compatibilidade futura)
    /// </summary>
    public int Version { get; } = 1;

    /// <summary>
    /// Construtor base inicializa Id e OccurredOnUtc
    /// </summary>
    protected IntegrationEvent()
    {
        Id = Guid.NewGuid();
        OccurredOnUtc = DateTime.UtcNow;
    }
}