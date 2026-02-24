using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.Contexts;
using GBastos.Casa_dos_Farelos.PagamentoService.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Outbox;

public sealed class IntegrationEventOutbox : IIntegrationEventOutbox
{
    private readonly PagamentoDbContext _context;

    public IntegrationEventOutbox(PagamentoDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(
        PagamentoAprovadoIntegrationEvent integrationEvent,
        CancellationToken cancellationToken)
    {
        var message = OutboxMessagePG.Create(integrationEvent);

        _context.OutboxMessages.Add(message);

        return Task.CompletedTask;
    }
}