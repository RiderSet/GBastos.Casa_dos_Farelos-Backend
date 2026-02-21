using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Contexts;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly PagamentoDbContext _context;

    public UnitOfWork(PagamentoDbContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}