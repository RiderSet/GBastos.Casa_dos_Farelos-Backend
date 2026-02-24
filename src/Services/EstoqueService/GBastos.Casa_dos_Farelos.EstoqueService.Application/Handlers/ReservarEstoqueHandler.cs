using GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Handlers;

public class ReservarEstoqueHandler :
    IRequestHandler<ReservarEstoqueCommand, bool>
{
    private readonly IEstoqueRepository _repo;
    private readonly IDistributedLockManager _lock;

    public ReservarEstoqueHandler(
        IEstoqueRepository repo,
        IDistributedLockManager lockManager)
    {
        _repo = repo;
        _lock = lockManager;
    }

    public async Task<bool> Handle(
        ReservarEstoqueCommand request,
        CancellationToken ct)
    {
        var lockKey = $"estoque:{request.ProdutoId}";

        await using var redLock =
            await _lock.AcquireLockAsync(lockKey, TimeSpan.FromSeconds(10));

        if (!redLock.IsAcquired)
            throw new Exception("Lock não adquirido");

        var estoque = await _repo.GetByProdutoIdAsync(request.ProdutoId);

        if (estoque is null)
            throw new InvalidOperationException(
                $"Estoque não encontrado para o produto {request.ProdutoId}");

        estoque.Debitar(request.Quantidade);

        await _repo.UpdateAsync(estoque);
        await _repo.SaveChangesAsync();

        return true;
    }
}