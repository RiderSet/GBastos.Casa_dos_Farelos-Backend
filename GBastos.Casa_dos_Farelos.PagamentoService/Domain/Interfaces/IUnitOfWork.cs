namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}