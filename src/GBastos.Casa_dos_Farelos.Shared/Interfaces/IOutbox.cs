namespace GBastos.Casa_dos_Farelos.Shared.Interfaces;

public interface IOutbox
{
    Task AddAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}