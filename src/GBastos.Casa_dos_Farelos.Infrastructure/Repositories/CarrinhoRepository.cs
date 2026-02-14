using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Repositories;

public class CarrinhoRepository : ICarrinhoRepository
{
    private readonly AppDbContext _db;

    public CarrinhoRepository(AppDbContext db) => _db = db;

    public async Task<Carrinho?> ObterPorClienteIdAsync(Guid clienteId)
    {
        return await _db.Carrinhos
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.ClienteId == clienteId);
    }

    public async Task AdicionarAsync(Carrinho carrinho)
    {
        await _db.Carrinhos.AddAsync(carrinho);
        await _db.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Carrinho carrinho)
    {
        _db.Carrinhos.Update(carrinho);
        await _db.SaveChangesAsync();
    }

    public async Task RemoverAsync(Carrinho carrinho)
    {
        _db.Carrinhos.Remove(carrinho);
        await _db.SaveChangesAsync();
    }
}