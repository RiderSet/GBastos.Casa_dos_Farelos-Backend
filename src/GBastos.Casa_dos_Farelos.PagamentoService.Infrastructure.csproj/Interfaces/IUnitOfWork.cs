using Microsoft.EntityFrameworkCore.Storage;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Interfaces;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct);
    Task CommitAsync(CancellationToken ct);
}