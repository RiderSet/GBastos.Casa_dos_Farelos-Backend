using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public interface IReservaRepository
{
    Task<Reserva?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Reserva reserva, CancellationToken ct);
    void Update(Reserva reserva);
}