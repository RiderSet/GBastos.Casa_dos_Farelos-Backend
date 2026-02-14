using GBastos.Casa_dos_Farelos.Application.Dtos;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Repositories;

public class CompraRepository : ICompraRepository
{
    private readonly AppDbContext _db;

    public CompraRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Compra?> ObterPorIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.Compras
            .Include(c => c.Itens)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<List<Compra>> ObterPorPeriodoAsync(DateTime? inicio, DateTime? fim, CancellationToken ct)
    {
        var query = _db.Compras.Include(c => c.Itens).AsNoTracking().AsQueryable();

        if (inicio.HasValue)
            query = query.Where(c => c.DataCompra >= inicio.Value);
        if (fim.HasValue)
            query = query.Where(c => c.DataCompra <= fim.Value);

        return await query.ToListAsync(ct);
    }

    public async Task AdicionarAsync(Compra compra, CancellationToken ct)
    {
        _db.Add(compra);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<CompraDto?> ObterDetalhadaPorIdAsync(Guid id, CancellationToken ct)
    {
        var compra = await _db.Compras
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CompraDto(
                c.Id,
                c.FornecedorId,
                c.TotalCompra,
                _db.ItensCompra
                    .Where(i => i.CompraId == c.Id)
                    .Join(_db.Produtos,
                        item => item.ProdutoId,
                        produto => produto.Id,
                        (item, produto) => new CompraItemDto(
                            item.ProdutoId,
                            produto.Nome,
                            item.Quantidade,
                            item.CustoUnitario,
                            item.SubTotal
                        ))
                    .ToList()
            ))
            .FirstOrDefaultAsync(ct);

        return compra;
    }
}

