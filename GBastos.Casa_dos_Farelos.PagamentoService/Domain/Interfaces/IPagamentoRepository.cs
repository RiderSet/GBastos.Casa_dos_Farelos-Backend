using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Interfaces;

public interface IPagamentoRepository
{
    Task AddAsync(Pagamento pagamento, CancellationToken cancellationToken = default);
    Task<Pagamento?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Pagamento?> GetByPedidoIdAsync(Guid pedidoId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByPedidoIdAsync(Guid pedidoId, CancellationToken cancellationToken = default);
    void Update(Pagamento pagamento);
}