using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Repositories;

public sealed class VendaRepository : IVendaRepository
{
    private readonly AppDbContext _db;

    public VendaRepository(AppDbContext db)
    {
        _db = db;
    }

    public IQueryable<Venda> Query()
        => _db.Vendas.AsQueryable();

    public async Task AddAsync(Venda venda, CancellationToken ct)
    {
        if (venda == null)
            throw new ArgumentNullException(nameof(venda));

        await _db.Vendas.AddAsync(venda, ct);
    }

    public Task UpdateAsync(Venda venda, CancellationToken ct)
    {
        if (venda == null)
            throw new ArgumentNullException(nameof(venda));

        _db.Vendas.Update(venda);
        return Task.CompletedTask;
    }

    public async Task<Venda?> ObterPorIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.Vendas
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id, ct);
    }

    /// <summary>
    /// Carrega o agregado completo (Itens + Produto)
    /// Usado para regras de negócio, cancelamentos, eventos, etc
    /// </summary>
    public async Task<Venda?> ObterCompletaAsync(Guid id, CancellationToken ct)
    {
        return await _db.Vendas
            .Include(v => v.Itens)
                .ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(v => v.Id == id, ct);
    }

    public async Task<bool> ExisteAsync(Guid id, CancellationToken ct)
    {
        return await _db.Vendas.AnyAsync(v => v.Id == id, ct);
    }
}