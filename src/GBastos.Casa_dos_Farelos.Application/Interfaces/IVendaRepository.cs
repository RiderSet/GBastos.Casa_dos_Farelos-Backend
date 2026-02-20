using GBastos.Casa_dos_Farelos.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IVendaRepository
{
    Task AddAsync(Venda venda, CancellationToken ct);
    Task UpdateAsync(Venda venda, CancellationToken ct);
    Task<Venda?> ObterPorIdAsync(Guid id, CancellationToken ct);
    Task<Venda?> ObterCompletaAsync(Guid id, CancellationToken ct);
    Task<bool> ExisteAsync(Guid id, CancellationToken ct);

    IQueryable<Venda> Query();
}