using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxMessage : Entity
{
    public string Type { get; private set; } = null!;
    public string Payload { get; private set; } = null!;
    public DateTime OccurredOn { get; private set; }
    public DateTime? ProcessedOn { get; private set; }
    public string? Error { get; private set; }

    public bool IsProcessed => ProcessedOn.HasValue;

    private OutboxMessage() { }

    public OutboxMessage(string type, string payload)
    {
        Id = Guid.NewGuid();
        Type = type;
        Payload = payload;
        OccurredOn = DateTime.UtcNow;
    }

    public void MarkAsProcessed()
    {
        ProcessedOn = DateTime.UtcNow;
        Error = null;
    }

    public void SetError(string error)
    {
        Error = error;
    }
}