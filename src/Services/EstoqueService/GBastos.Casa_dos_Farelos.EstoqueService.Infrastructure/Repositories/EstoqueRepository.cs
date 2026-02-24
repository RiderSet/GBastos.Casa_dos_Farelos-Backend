using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Repositories;

public class EstoqueRepository : IEstoqueRepository
{
    private readonly EstoqueDbContext _context;

    public EstoqueRepository(EstoqueDbContext context)
    {
        _context = context;
    }

    public async Task<ProdutoEstoque?> GetByProdutoIdAsync(Guid produtoId)
    {
        return await _context.Produtos
            .FirstOrDefaultAsync(x => x.ProdutoId == produtoId);
    }

    public Task UpdateAsync(ProdutoEstoque estoque)
    {
        _context.Produtos.Update(estoque);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task ExpireReservasAsync()
    {
        throw new NotImplementedException();
    }
}