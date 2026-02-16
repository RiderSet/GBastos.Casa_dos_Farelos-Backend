using GBastos.Casa_dos_Farelos.Domain.Common;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxMessage : BaseEntity
{
    public DateTime OccurredOn { get; private set; }
    public string EventName { get; private set; } = null!;
    public string Payload { get; private set; } = null!;
    public DateTime? ProcessedOn { get; private set; }
    public string? Error { get; private set; }

    public bool IsProcessed => ProcessedOn.HasValue;

    private OutboxMessage() { }

    private OutboxMessage(Guid id, string eventName, string payload, DateTime occurredOnUtc)
    {
        Id = id;
        EventName = eventName;
        Payload = payload;
        OccurredOn = occurredOnUtc;
    }

    public static OutboxMessage CreateIntegrationEvent(object integrationEvent)
    {
        var eventName = integrationEvent.GetType().Name;

        return new OutboxMessage(
            Guid.NewGuid(),
            eventName,
            JsonSerializer.Serialize(integrationEvent),
            DateTime.UtcNow
        );
    }

    public static OutboxMessage CreateDomainEvent(object domainEvent)
    {
        var eventName = domainEvent.GetType().Name;

        return new OutboxMessage(
            Guid.NewGuid(),
            eventName,
            JsonSerializer.Serialize(domainEvent),
            DateTime.UtcNow
        );
    }

    public void MarkProcessed()
    {
        ProcessedOn = DateTime.UtcNow;
        Error = null;
    }

    public void MarkFailed(string error)
    {
        Error = error;
    }
}