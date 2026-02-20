namespace GBastos.Casa_dos_Farelos.Application.Common;

/// <summary>
/// Representa uma mensagem do Outbox transportada da Infrastructure para Application
/// </summary>
public sealed class OutboxItem
{
    public Guid Id { get; init; }
    public DateTime OccurredOn { get; init; }
    public string Type { get; init; } = string.Empty;
    public string Payload { get; init; } = string.Empty;
}