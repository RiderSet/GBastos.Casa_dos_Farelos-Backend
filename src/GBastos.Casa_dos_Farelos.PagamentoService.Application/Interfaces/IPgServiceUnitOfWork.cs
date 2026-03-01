using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Interfaces;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Application.Interfaces;

public interface IPgServiceUnitOfWork
{
    IReservaRepository Reservas { get; }
    IOutboxRepository Outbox { get; }
    IIdempotencyRepository Idempotency { get; }

    Task BeginTransactionAsync(CancellationToken ct);
    Task CommitAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}