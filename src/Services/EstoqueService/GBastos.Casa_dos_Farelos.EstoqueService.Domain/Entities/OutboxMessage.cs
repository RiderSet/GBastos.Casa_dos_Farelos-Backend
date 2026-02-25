namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;

public sealed class OutboxMessage
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Type { get; private set; } = default!;
    public string Payload { get; private set; } = default!;

    public DateTime OccurredOn { get; private set; } = DateTime.UtcNow;
    public DateTime? ProcessedOn { get; private set; }

    public int RetryCount { get; private set; }
    public string? Error { get; private set; }

    private OutboxMessage() { }

    public OutboxMessage(string type, string payload)
    {
        Type = type;
        Payload = payload;
    }

    public void MarkProcessed()
    {
        ProcessedOn = DateTime.UtcNow;
    }

    public void MarkFailed(string error)
    {
        RetryCount++;
        Error = error;
    }

    public bool IsDeadLetter(int maxRetries)
        => RetryCount >= maxRetries;
}