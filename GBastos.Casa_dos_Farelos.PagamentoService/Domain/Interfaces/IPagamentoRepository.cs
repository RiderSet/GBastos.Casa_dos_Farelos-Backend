using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Interfaces;

public interface IPagamentoRepository
{
    Task AddAsync(Pagamento pagamento, CancellationToken ct);
    Task<Pagamento?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Pagamento?> ObterPorPedidoIdAsync(Guid pedidoId, CancellationToken ct);
    Task<Pagamento?> ObterPorIdempotencyKeyAsync(string key, CancellationToken ct);
    Task<bool> ExistsByPedidoIdAsync(Guid pedidoId, CancellationToken ct);
    Task<Pagamento?> ObterParaLockAsync(Guid pedidoId, CancellationToken ct);
    void Update(Pagamento pagamento);
}