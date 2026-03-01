using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.ComprasService.Infrastructure.Outbox;

public class OutboxMessage
{
    public Guid Id { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public DateTime OccurredOnUtc { get; private set; }
    public DateTime? ProcessedOnUtc { get; private set; }

    private OutboxMessage() { }

    public OutboxMessage(object @event)
    {
        Id = Guid.NewGuid();
        Type = @event.GetType().FullName!;
        Content = JsonSerializer.Serialize(@event);
        OccurredOnUtc = DateTime.UtcNow;
    }

    public void MarkAsProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow;
    }
}