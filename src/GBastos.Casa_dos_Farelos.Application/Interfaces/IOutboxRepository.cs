using GBastos.Casa_dos_Farelos.Application.Common;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IOutboxRepository
{
    Task AddAsync(IIntegrationEvent @event, CancellationToken ct);

    Task<IReadOnlyList<OutboxItem>> GetPendingAsync(int batchSize, CancellationToken ct);

    Task MarkAsProcessedAsync(Guid id, CancellationToken ct);

    Task MarkAsFailedAsync(Guid id, string error, CancellationToken ct);
}