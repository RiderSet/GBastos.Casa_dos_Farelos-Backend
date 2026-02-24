using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Repositories;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.UOW;

public sealed class EstoqueUnitOfWork : IEstoqueUnitOfWork
{
    private readonly EstoqueDbContext _context;

    public EstoqueUnitOfWork(EstoqueDbContext context)
    {
        _context = context;
    }

    public IReservaRepository Reservas =>
        new ReservaRepository(_context);

    public IOutboxRepository Outbox =>
        new OutboxRepository(_context);

    public IIdempotencyRepository Idempotency =>
        new IdempotencyRepository(_context);

    public async Task BeginTransactionAsync(CancellationToken ct)
        => await _context.Database.BeginTransactionAsync(ct);

    public async Task CommitAsync(CancellationToken ct)
        => await _context.Database.CommitTransactionAsync(ct);

    public async Task SaveChangesAsync(CancellationToken ct)
        => await _context.SaveChangesAsync(ct);
}