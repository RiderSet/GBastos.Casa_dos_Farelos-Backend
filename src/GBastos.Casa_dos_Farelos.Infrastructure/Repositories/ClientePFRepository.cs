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

    private IQueryable<ClientePF> ClientesPF => _db.Pessoas.OfType<ClientePF>();

    public async Task AddAsync(ClientePF cliente, CancellationToken ct)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));

        await _db.Pessoas.AddAsync(cliente, ct);
    }

    public Task UpdateAsync(ClientePF cliente, CancellationToken ct)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));

        _db.Pessoas.Update(cliente);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(ClientePF cliente, CancellationToken ct)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));

        _db.Pessoas.Remove(cliente);
        return Task.CompletedTask;
    }

    public async Task<ClientePF?> ObterPorIdAsync(Guid id, CancellationToken ct)
    {
        return await ClientesPF
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<ClientePF?> ObterPorCpfAsync(string cpf, CancellationToken ct)
    {
        return await ClientesPF
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CPF == cpf, ct);
    }

    public async Task<bool> ExistePorCpfAsync(string cpf, CancellationToken ct)
    {
        return await ClientesPF
            .AnyAsync(c => c.CPF == cpf, ct);
    }

    //async Task<List<ClienteListDto>> ListarAsync(CancellationToken ct)
    //{
    //    return await ClientesPF
    //        .Select(c => new ClienteListDto(
    //            c.Id,
    //            c.Nome,
    //            c.Telefone,
    //            c.Email,
    //            "PF",
    //            c.CPF,
    //            null))
    //        .ToListAsync(ct);
    //}

    //Task<List<ClienteListDto>> IClientePFRepository.ListarAsync(CancellationToken ct)
    //{
    //    return ListarAsync(ct);
    //}
}