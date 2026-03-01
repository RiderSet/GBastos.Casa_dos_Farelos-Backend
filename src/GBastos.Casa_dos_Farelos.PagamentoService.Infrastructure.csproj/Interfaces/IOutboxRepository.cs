using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Outbox;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Interfaces;

public interface IOutboxRepository
{
    Task<List<OutboxMessagePG>> GetPendingAsync(
        int take,
        CancellationToken ct);

    Task MarkAsProcessedAsync(
        Guid id,
        CancellationToken ct);

    Task MarkAsFailedAsync(
        Guid id,
        string error,
        CancellationToken ct);

    Task AddAsync<T>(
        T @event,
        CancellationToken ct);
}