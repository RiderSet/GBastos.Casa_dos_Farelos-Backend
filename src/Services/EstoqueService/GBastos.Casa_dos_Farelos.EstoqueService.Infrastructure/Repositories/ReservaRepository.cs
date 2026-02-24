using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Repositories;

public sealed class ReservaRepository : IReservaRepository
{
    private readonly EstoqueDbContext _context;

    public ReservaRepository(EstoqueDbContext context)
    {
        _context = context;
    }

    public async Task<Reserva?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _context.Reservas
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task AddAsync(Reserva reserva, CancellationToken ct)
        => await _context.Reservas.AddAsync(reserva, ct);

    public void Update(Reserva reserva)
        => _context.Reservas.Update(reserva);
}