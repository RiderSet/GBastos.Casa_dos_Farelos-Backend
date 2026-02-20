using GBastos.Casa_dos_Farelos.Domain.Dtos;
using GBastos.Casa_dos_Farelos.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IClienteRepository
{
    Task AddAsync(Pessoa pessoa, CancellationToken ct = default);
    Task<Pessoa?> ObterPorIdAsync(Guid id, CancellationToken ct = default);
    Task<List<ClienteListDto>> ListarAsync(CancellationToken ct);
}