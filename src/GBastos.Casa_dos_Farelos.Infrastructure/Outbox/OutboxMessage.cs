using GBastos.Casa_dos_Farelos.Domain.Abstractions;
using GBastos.Casa_dos_Farelos.Domain.Common;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxMessage : BaseEntity
{
    public DateTime OccurredOnUtc { get; private set; }
    public string EventName { get; private set; } = null!;
    public string Payload { get; private set; } = null!;
    public DateTime? ProcessedOnUtc { get; private set; }
    public string? Error { get; private set; }

    public bool IsProcessed => ProcessedOnUtc.HasValue;

    private OutboxMessage() { }

    public OutboxMessage(Guid id, string eventName, string payload, DateTime utcNow)
    {
        Id = id;
        EventName = eventName;
        Payload = payload;
        ProcessedOnUtc = utcNow;
    }

    public static OutboxMessage Create(IDomainEvent domainEvent)
    {
        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredOnUtc = domainEvent.OccurredOnUtc,
            EventName = domainEvent.GetType().AssemblyQualifiedName!,
            Payload = JsonSerializer.Serialize(domainEvent)
        };
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
        ProcessedOnUtc = DateTime.UtcNow;
        Error = null;
    }

    public void MarkFailed(string error)
    {
        Error = error;
    }
}