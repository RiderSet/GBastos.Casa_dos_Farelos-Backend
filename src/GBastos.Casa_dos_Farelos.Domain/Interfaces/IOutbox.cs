namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IOutbox
{
    Task AddAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
}