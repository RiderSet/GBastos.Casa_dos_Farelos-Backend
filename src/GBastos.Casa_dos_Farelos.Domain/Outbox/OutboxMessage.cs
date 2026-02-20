using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Outbox;

/// <summary>
/// Mensagem de Outbox para envio assíncrono de eventos de domínio.
/// </summary>
public sealed class OutboxMessage : Entity
{
    public string Type { get; private set; } = null!;
    public string Payload { get; private set; } = null!;
    public DateTime OccurredOnUtc { get; private set; }
    public DateTime? ProcessedOn { get; private set; }
    public int Attempts { get; private set; }
    public string? ErrorMessage { get; private set; } // ← nova propriedade

    private OutboxMessage() { } // EF Core

    public OutboxMessage(string type, string payload)
    {
        Id = Guid.NewGuid();
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Payload = payload ?? throw new ArgumentNullException(nameof(payload));
        OccurredOnUtc = DateTime.UtcNow;
        Attempts = 0;
    }

    /// <summary>
    /// Marca a mensagem como processada.
    /// </summary>
    public void MarkAsProcessed()
    {
        ProcessedOn = DateTime.UtcNow;
        Attempts++;
        ErrorMessage = null; // limpa erro anterior, se houver
    }

    /// <summary>
    /// Registra um erro de processamento.
    /// </summary>
    public void SetError(string message)
    {
        ErrorMessage = message;
        Attempts++;
    }

    public bool IsProcessed => ProcessedOn.HasValue;
}
