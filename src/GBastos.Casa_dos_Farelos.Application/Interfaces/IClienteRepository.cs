using GBastos.Casa_dos_Farelos.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IClienteRepository
{
    Task AddAsync(Cliente cliente, CancellationToken ct);
    Task<Cliente?> GetByIdAsync(Guid id, CancellationToken ct);
}