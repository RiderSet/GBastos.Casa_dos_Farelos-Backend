using Microsoft.EntityFrameworkCore.Storage;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct);
    Task CommitAsync(CancellationToken ct);
}