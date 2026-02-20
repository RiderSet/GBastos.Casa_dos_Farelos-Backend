namespace GBastos.Casa_dos_Farelos.Domain.Interfaces;

public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTime OccurredOn { get; }
}
