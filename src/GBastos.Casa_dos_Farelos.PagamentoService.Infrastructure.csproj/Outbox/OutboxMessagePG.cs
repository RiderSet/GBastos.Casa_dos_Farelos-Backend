using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Common;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Outbox;

public sealed class OutboxMessagePG : BaseEntity
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

    private OutboxMessagePG() { }

    private OutboxMessagePG(
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

    public static OutboxMessagePG Create(PagamentoAprovadoIntegrationEvent integrationEvent)
    {
        return new OutboxMessagePG(
            Guid.NewGuid(),
            integrationEvent.GetType().AssemblyQualifiedName!,
            JsonSerializer.Serialize(integrationEvent),
            integrationEvent.CriadoEmUtc
        );
    }

    public static OutboxMessagePG CreateIntegrationEvent(object integrationEvent)
    {
        return new OutboxMessagePG(
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
}