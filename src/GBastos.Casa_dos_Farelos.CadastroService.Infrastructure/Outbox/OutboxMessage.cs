using GBastos.Casa_dos_Farelos.CadastroService.Domain.Common;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Outbox;

public sealed class OutboxMessage : BaseEntity
{
    public DateTime OccurredOnUtc { get; private set; }
    public string Type { get; private set; } = null!;
    public string Payload { get; private set; } = null!;
    public DateTime? ProcessedOnUtc { get; private set; }
    public int Attempts { get; private set; }
    public string? Error { get; private set; }
    public int RetryCount { get; private set; }

    // Controle de concorrência
    public Guid? LockId { get; private set; }
    public DateTime? LockedUntilUtc { get; private set; }

    private OutboxMessage() { }

    private OutboxMessage(
        Guid id,
        string eventType,
        string payload,
        DateTime occurredOnUtc)
    {
        Id = id;
        Type = eventType;
        Payload = payload;
        OccurredOnUtc = occurredOnUtc;
    }

    public static OutboxMessage Create(IDomainEvent domainEvent)
    {
        return new OutboxMessage(
            Guid.NewGuid(),
            domainEvent.GetType().AssemblyQualifiedName!,
            JsonSerializer.Serialize(domainEvent),
            domainEvent.OccurredOnUtc
        );
    }

    public static OutboxMessage CreateIntegrationEvent(object integrationEvent)
    {
        return new OutboxMessage(
            Guid.NewGuid(),
            integrationEvent.GetType().AssemblyQualifiedName!,
            JsonSerializer.Serialize(integrationEvent),
            DateTime.UtcNow
        );
    }

    public void MarkAsProcessed()
    {
        ProcessedOnUtc = DateTime.UtcNow;
        Error = null;
    }

    public void MarkFailed(string error)
    {
        Attempts++;
        Error = error;
    }

    public void Lock(Guid lockId, TimeSpan duration)
    {
        LockId = lockId;
        LockedUntilUtc = DateTime.UtcNow.Add(duration);
    }

    public void Unlock()
    {
        LockId = null;
        LockedUntilUtc = null;
    }

    public bool IsProcessed => ProcessedOnUtc.HasValue;

    public JsonDocument Content { get; set; }
}
