using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Outbox;


public sealed class IntegrationEventOutbox : IIntegrationEventOutbox
{
    private readonly IOutboxRepository _repository;

    public IntegrationEventOutbox(IOutboxRepository repository)
    {
        _repository = repository;
    }

    public async Task AddAsync(PagamentoAprovadoIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        await _repository.AddAsync(integrationEvent, cancellationToken);
    }
}