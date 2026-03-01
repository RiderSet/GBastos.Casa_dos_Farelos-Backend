using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Inbox;

public sealed class InboxMessage
{
    public Guid Id { get; private set; }           // Id do IntegrationEvent
    public string EventType { get; private set; }  // Nome do evento
    public string Content { get; private set; }    // JSON
    public DateTime ReceivedAtUtc { get; private set; }
    public DateTime? ProcessedAtUtc { get; private set; }

    private InboxMessage() { } // EF

    public InboxMessage(Guid id, string eventType, object content)
    {
        Id = id;
        EventType = eventType;
        Content = JsonSerializer.Serialize(content);
        ReceivedAtUtc = DateTime.UtcNow;
    }

    public void MarkAsProcessed()
    {
        ProcessedAtUtc = DateTime.UtcNow;
    }

    public bool IsProcessed => ProcessedAtUtc.HasValue;
}