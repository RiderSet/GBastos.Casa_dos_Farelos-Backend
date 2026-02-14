using GBastos.Casa_dos_Farelos.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IClienteRepository
{
    Task AdicionarClientePFAsync(ClientePF cliente, CancellationToken ct);
    Task AdicionarClientePJAsync(ClientePJ cliente, CancellationToken ct);
    Task<ClientePF?> ObterClientePFPorIdAsync(Guid id, CancellationToken ct);
    Task<ClientePJ?> ObterClientePJPorIdAsync(Guid id, CancellationToken ct);
}