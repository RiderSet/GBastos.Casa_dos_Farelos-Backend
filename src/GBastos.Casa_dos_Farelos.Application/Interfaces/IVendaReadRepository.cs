using GBastos.Casa_dos_Farelos.Application.Dtos;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IVendaReadRepository
{
    Task<VendaDetalhadaDto?> ObterPorIdAsync(Guid id, CancellationToken ct);
}
