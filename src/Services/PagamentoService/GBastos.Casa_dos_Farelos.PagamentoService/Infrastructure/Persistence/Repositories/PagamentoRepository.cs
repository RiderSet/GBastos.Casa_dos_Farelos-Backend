using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Entities;
using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.Repositories;

public class PagamentoRepository : IPagamentoRepository
{
    private readonly PagamentoDbContext _context;

    public PagamentoRepository(PagamentoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Pagamento pagamento,
        CancellationToken ct)
    {
        await _context.Pagamentos.AddAsync(pagamento, ct);
    }

    public async Task<Pagamento?> GetByIdAsync(
        Guid id,
        CancellationToken ct)
    {
        return await _context.Pagamentos
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<Pagamento?> ObterPorPedidoIdAsync(
        Guid pedidoId,
        CancellationToken ct)
    {
        return await _context.Pagamentos
            .FirstOrDefaultAsync(x => x.PedidoId == pedidoId, ct);
    }

    public async Task<Pagamento?> ObterPorIdempotencyKeyAsync(
        string idempotencyKey,
        CancellationToken ct)
    {
        return await _context.Pagamentos
            .FirstOrDefaultAsync(x =>
                x.IdempotencyKey == idempotencyKey,
                ct);
    }

    public async Task<bool> ExistsByPedidoIdAsync(
        Guid pedidoId,
        CancellationToken ct)
    {
        return await _context.Pagamentos
            .AnyAsync(x => x.PedidoId == pedidoId, ct);
    }

    public async Task<Pagamento?> ObterParaLockAsync(
        Guid pedidoId,
        CancellationToken ct)
    {
        return await _context.Pagamentos
            .FromSqlRaw("""
                SELECT *
                FROM Pagamentos WITH (UPDLOCK, ROWLOCK)
                WHERE PedidoId = {0}
            """, pedidoId)
            .FirstOrDefaultAsync(ct);
    }

    public void Update(Pagamento pagamento)
    {
        _context.Pagamentos.Update(pagamento);
    }
}