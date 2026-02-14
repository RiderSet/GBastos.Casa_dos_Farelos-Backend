using GBastos.Casa_dos_Farelos.Domain.Common;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxMessage : BaseEntity
{
    public DateTime OccurredOn { get; private set; }
    public string Type { get; private set; } = null!;
    public string Payload { get; private set; } = null!;

    public DateTime? ProcessedOn { get; private set; }
    public string? Error { get; private set; }

    public bool IsProcessed => ProcessedOn.HasValue;

    private OutboxMessage() { } // EF

    private OutboxMessage(Guid id, DateTime occurredOn, string type, string payload)
    {
        Id = id;
        OccurredOn = occurredOn;
        Type = type;
        Payload = payload;
    }

    public static OutboxMessage Create(object domainEvent, Guid id, DateTime occurredOn)
    {
        return new OutboxMessage
        {
            Id = id,
            Type = domainEvent.GetType().Name,
            Payload = JsonSerializer.Serialize(domainEvent),
            OccurredOn = occurredOn,
            ProcessedOn = null,
            Error = null
        };
    }

    public void MarkAsProcessed()
    {
        ProcessedOn = DateTime.UtcNow;
        Error = null;
    }

    public void MarkAsFailed(string error)
    {
        Error = error;
        ProcessedOn = null;
    }

    public void SetError(string error)
    {
        Error = error;
    }
}