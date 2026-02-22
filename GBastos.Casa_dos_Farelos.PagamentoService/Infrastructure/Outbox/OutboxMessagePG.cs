using System.ComponentModel.DataAnnotations;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Outbox;

public sealed class OutboxMessagePG
{
    public Guid Id { get; private set; }
    public string Type { get; private set; } = default!;
    public string Content { get; private set; } = default!;
    public DateTime OccurredOnUtc { get; private set; }
    public DateTime? NextAttemptUtc { get; private set; }
    public DateTime? ProcessedOnUtc { get; private set; }
    public int RetryCount { get; private set; }
    public string? Error { get; private set; }

    [Timestamp]
    public byte[] RowVersion { get; private set; } = default!;

    private OutboxMessagePG() { } // EF Core

    private OutboxMessagePG(
        Guid id,
        string type,
        string content,
        DateTime occurredOnUtc)
    {
        Id = id;
        Type = type;
        Content = content;
        OccurredOnUtc = occurredOnUtc;
    }

    public static OutboxMessagePG Create(object integrationEvent)
    {
        return new OutboxMessagePG(
            Guid.NewGuid(),
            integrationEvent.GetType().FullName!,
            System.Text.Json.JsonSerializer.Serialize(integrationEvent),
            DateTime.UtcNow);
    }

    public void MarkAsProcessed()
        => ProcessedOnUtc = DateTime.UtcNow;

    public void MarkAsFailed(string error)
    {
        RetryCount++;
        Error = error;
    }

    internal void ScheduleNextAttempt(TimeSpan timeSpan)
    {
        NextAttemptUtc = DateTime.UtcNow.Add(timeSpan);
    }
}