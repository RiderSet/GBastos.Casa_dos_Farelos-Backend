using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public interface IEstoqueRepository
{
    Task<ProdutoEstoque?> GetByProdutoIdAsync(Guid id);
    Task UpdateAsync(ProdutoEstoque estoque);
    Task SaveChangesAsync();
    Task ExpireReservasAsync();
}