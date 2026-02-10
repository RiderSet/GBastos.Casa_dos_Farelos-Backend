using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Repositories;

public sealed class ClientePFRepository : IClientePFRepository
{
    private readonly AppDbContext _db;

    public ClientePFRepository(AppDbContext db)
    {
        _db = db;
    }

    // ---------------- COMMAND SIDE ----------------

    public async Task AddAsync(ClientePF cliente, CancellationToken ct)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));

        await _db.ClientesPF.AddAsync(cliente, ct);
    }

    public Task UpdateAsync(ClientePF cliente, CancellationToken ct)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));

        _db.ClientesPF.Update(cliente);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(ClientePF cliente, CancellationToken ct)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));

        _db.ClientesPF.Remove(cliente);
        return Task.CompletedTask;
    }

    // ---------------- QUERY SIDE ----------------

    public async Task<ClientePF?> ObterPorIdAsync(Guid id, CancellationToken ct)
    {
        return await _db.ClientesPF
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<ClientePF?> ObterPorCpfAsync(string cpf, CancellationToken ct)
    {
        return await _db.ClientesPF
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CPF == cpf, ct);
    }

    public async Task<List<ClientePF>> ListarAsync(CancellationToken ct)
    {
        return await _db.ClientesPF
            .AsNoTracking()
            .OrderBy(c => c.Nome)
            .ToListAsync(ct);
    }

    public async Task<bool> ExistePorCpfAsync(string cpf, CancellationToken ct)
    {
        return await _db.ClientesPF
            .AnyAsync(c => c.CPF == cpf, ct);
    }
}