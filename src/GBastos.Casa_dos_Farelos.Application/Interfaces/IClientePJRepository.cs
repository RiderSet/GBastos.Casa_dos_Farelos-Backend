
using GBastos.Casa_dos_Farelos.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IClientePJRepository
{
    Task AddAsync(ClientePJ cliente, CancellationToken ct);
    Task UpdateAsync(ClientePJ cliente, CancellationToken ct);
    Task RemoveAsync(ClientePJ cliente, CancellationToken ct);

    Task<ClientePJ?> ObterPorIdAsync(Guid id, CancellationToken ct);
    Task<ClientePJ?> ObterPorCpfAsync(string cpf, CancellationToken ct);
    Task<List<ClientePJ>> ListarAsync(CancellationToken ct);
    Task<bool> ExistePorCpfAsync(string cpf, CancellationToken ct);
}

