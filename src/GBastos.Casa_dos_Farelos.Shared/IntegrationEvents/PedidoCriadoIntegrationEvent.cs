using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

/// <summary>
/// Evento disparado quando um pedido é criado.
/// </summary>
public sealed record PedidoCriadoIntegrationEvent(
    Guid Id,
    Guid PedidoId,
    Guid ClienteId,
    decimal Total,
    DateTime OccurredOnUtc
) : IIntegrationEvent
{
    // Nome do tipo do evento
    public string EventType => nameof(PedidoCriadoIntegrationEvent);

    // Versão do evento (para compatibilidade futura)
    public int Version { get; } = 1;

    /// <summary>
    /// Construtor auxiliar que inicializa Id e OccurredOnUtc automaticamente
    /// </summary>
    public PedidoCriadoIntegrationEvent(Guid pedidoId, Guid clienteId, decimal total)
        : this(Guid.NewGuid(), pedidoId, clienteId, total, DateTime.UtcNow)
    {
    }
}