using GBastos.Casa_dos_Farelos.Domain.Dtos;
using GBastos.Casa_dos_Farelos.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface ICompraRepository
{
    Task<Compra?> ObterPorIdAsync(Guid id, CancellationToken ct);
    Task<List<Compra>> ObterPorPeriodoAsync(DateTime? inicio, DateTime? fim, CancellationToken ct);
    Task AdicionarAsync(Compra compra, CancellationToken ct);
    Task<CompraDto?> ObterDetalhadaPorIdAsync(Guid id, CancellationToken ct);
}