namespace GBastos.Casa_dos_Farelos.Shared.Interfaces;

public interface IIntegrationEventOutbox
{
    Task AddAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}