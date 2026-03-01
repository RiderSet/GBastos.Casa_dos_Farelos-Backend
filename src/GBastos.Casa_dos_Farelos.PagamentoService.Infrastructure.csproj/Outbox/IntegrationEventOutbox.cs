using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Outbox;

public sealed class IntegrationEventOutbox : IIntegrationEventOutbox
{
    private readonly IOutboxRepository _repository;

    public IntegrationEventOutbox(IOutboxRepository repository)
    {
        _repository = repository;
    }

    public async Task AddAsync(object integrationEvent, CancellationToken cancellationToken)
    {
        await _repository.AddAsync(integrationEvent, cancellationToken);
    }
}