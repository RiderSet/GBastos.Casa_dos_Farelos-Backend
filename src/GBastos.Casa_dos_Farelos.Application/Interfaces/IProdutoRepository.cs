using GBastos.Casa_dos_Farelos.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IProdutoRepository
{
    // COMMAND SIDE
    Task AddAsync(Produto produto, CancellationToken ct);
    Task UpdateAsync(Produto produto, CancellationToken ct);
    Task RemoveAsync(Produto produto, CancellationToken ct);

    // QUERY SIDE (agregado)
    Task<Produto?> ObterPorIdAsync(Guid id, CancellationToken ct);
    Task<Dictionary<Guid, Produto>> ObterPorIdsAsync(IEnumerable<Guid> ids, CancellationToken ct);
    Task<bool> ExistePorNomeAsync(string nome, CancellationToken ct);
}
