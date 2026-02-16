namespace GBastos.Casa_dos_Farelos.Shared.Interfaces;

public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
    string EventType { get; }
    int Version { get; }
}