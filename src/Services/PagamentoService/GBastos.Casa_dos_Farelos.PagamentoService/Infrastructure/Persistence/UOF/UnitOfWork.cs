using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Interfaces;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.UOF;

using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly PagamentoDbContext _context;

    public UnitOfWork(PagamentoDbContext context)
    {
        _context = context;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(
        CancellationToken ct)
    {
        return await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}