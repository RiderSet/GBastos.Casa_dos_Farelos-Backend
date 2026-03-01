using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Inbox;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Interfaces;

public interface IInboxRepository
{
    /// <summary>
    /// Verifica se a mensagem já foi recebida.
    /// Usado para idempotência.
    /// </summary>
    Task<bool> ExistsAsync(Guid messageId, CancellationToken cancellationToken);

    /// <summary>
    /// Recupera uma mensagem pelo Id.
    /// </summary>
    Task<InboxMessage?> GetByIdAsync(Guid messageId, CancellationToken cancellationToken);

    /// <summary>
    /// Adiciona uma nova mensagem na Inbox.
    /// </summary>
    Task AddAsync(InboxMessage message, CancellationToken cancellationToken);

    /// <summary>
    /// Marca uma mensagem como processada.
    /// </summary>
    Task MarkAsProcessedAsync(Guid messageId, CancellationToken cancellationToken);

    /// <summary>
    /// Persiste alterações.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken);
}