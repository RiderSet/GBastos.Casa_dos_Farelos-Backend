using GBastos.Casa_dos_Farelos.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IClientePFRepository
{
    Task AddAsync(ClientePF cliente, CancellationToken ct);
    Task UpdateAsync(ClientePF cliente, CancellationToken ct);
    Task RemoveAsync(ClientePF cliente, CancellationToken ct);

    Task<ClientePF?> ObterPorIdAsync(Guid id, CancellationToken ct);
    Task<ClientePF?> ObterPorCpfAsync(string cpf, CancellationToken ct);

    Task<bool> ExistePorCpfAsync(string cpf, CancellationToken ct);

   // Task<List<ClienteListDto>> ListarAsync(CancellationToken ct);
}