using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Interfaces;

public interface IOutboxRepository
{
    Task<List<OutboxMessage>> GetPendingAsync(int take, CancellationToken ct);
    Task<List<OutboxMessage>> GetUnprocessedAsync(int take, CancellationToken ct);
    Task MarkAsProcessedAsync(Guid id, CancellationToken ct);
    Task MarkAsFailedAsync(Guid id, string error, CancellationToken ct);
    Task AddAsync<T>(T @event, CancellationToken ct)
        where T : IIntegrationEvent;

    Task SaveChangesAsync(CancellationToken ct);
}