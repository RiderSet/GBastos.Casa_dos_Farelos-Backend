using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Repositories;

public sealed class IdempotencyRepository : IIdempotencyRepository
{
    private readonly EstoqueDbContext _context;

    public IdempotencyRepository(EstoqueDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(string key, CancellationToken ct)
        => await _context.IdempotencyKeys
            .AnyAsync(x => x.Key == key, ct);

    public async Task AddAsync(string key, CancellationToken ct)
        => await _context.IdempotencyKeys
            .AddAsync(new IdempotencyKey(key), ct);
}