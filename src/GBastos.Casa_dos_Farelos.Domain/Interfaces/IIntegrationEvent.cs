namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}