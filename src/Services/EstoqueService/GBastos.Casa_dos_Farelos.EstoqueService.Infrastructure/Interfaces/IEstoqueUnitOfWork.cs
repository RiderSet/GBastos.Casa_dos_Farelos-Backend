using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Interfaces;

public interface IEstoqueUnitOfWork
{
    IReservaRepository Reservas { get; }
    IOutboxRepository Outbox { get; }
    IIdempotencyRepository Idempotency { get; }

    Task BeginTransactionAsync(CancellationToken ct);
    Task CommitAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}