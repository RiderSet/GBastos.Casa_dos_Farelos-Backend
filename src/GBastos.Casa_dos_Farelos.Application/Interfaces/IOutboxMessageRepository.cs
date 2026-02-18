using GBastos.Casa_dos_Farelos.Application.Common;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IOutboxMessageRepository
{
    /// <summary>
    /// Adiciona um evento de integração ao Outbox
    /// </summary>
    Task AddAsync(IIntegrationEvent @event, CancellationToken ct);

    /// <summary>
    /// Retorna mensagens ainda não processadas
    /// </summary>
    Task<IReadOnlyList<OutboxItem>> GetPendingAsync(int batchSize, CancellationToken ct);

    /// <summary>
    /// Marca a mensagem como processada com sucesso
    /// </summary>
    Task MarkAsProcessedAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Marca a mensagem como falha (para retry posterior)
    /// </summary>
    Task MarkAsFailedAsync(Guid id, string error, CancellationToken ct);
}