using GBastos.Casa_dos_Farelos.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IVendaSaveRepository
{
    Task AddAsync(Venda venda, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
