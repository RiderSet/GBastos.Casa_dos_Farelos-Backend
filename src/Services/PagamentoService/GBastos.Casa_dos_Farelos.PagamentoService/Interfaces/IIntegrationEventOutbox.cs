using GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Interfaces;

public interface IIntegrationEventOutbox
{
    Task AddAsync(PagamentoAprovadoIntegrationEvent integrationEvent, CancellationToken cancellationToken);
}
