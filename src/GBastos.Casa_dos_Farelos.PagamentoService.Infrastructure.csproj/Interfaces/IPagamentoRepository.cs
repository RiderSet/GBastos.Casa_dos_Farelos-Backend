using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Aggregates;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Interfaces;

public interface IPagamentoRepository
{
    Task<Pagamento?> ObterPorIdempotencyKeyAsync(
        string key,
        CancellationToken ct);

    Task<Pagamento?> ObterPorPedidoIdAsync(
        Guid pedidoId,
        CancellationToken ct);

    Task<Pagamento?> ObterParaLockAsync(
        Guid pedidoId,
        CancellationToken ct);

    Task AddAsync(Pagamento pagamento, CancellationToken ct);
}