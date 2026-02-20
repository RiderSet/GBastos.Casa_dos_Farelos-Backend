using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Repositories;

public sealed class ClientePJRepository : IClientePJRepository
{
    private readonly AppDbContext _db;

    public ClientePJRepository(AppDbContext db)
    {
        _db = db;
    }

    // IQueryable para consultar apenas os ClientesPJ (TPH)
    private IQueryable<ClientePJ> ClientesPJ => _db.Pessoas.OfType<ClientePJ>();

    public async Task AddAsync(ClientePJ cliente, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(cliente);

        await _db.Pessoas.AddAsync(cliente, ct);
    }

    public async Task<bool> ExistePorCnpjAsync(string cnpj, CancellationToken ct)
    {
        return await ClientesPJ
            .AsNoTracking()
            .AnyAsync(x => x.CNPJ == cnpj, ct);
    }

    public Task<bool> ExistePorCNPJAsync(string cnpj, CancellationToken ct)
    {
        // Redireciona para o método já implementado
        return ExistePorCnpjAsync(cnpj, ct);
    }

    public async Task<ClientePJ?> ObterPorCnpjAsync(string cnpj, CancellationToken ct)
    {
        return await ClientesPJ
            .FirstOrDefaultAsync(x => x.CNPJ == cnpj, ct);
    }

    public Task<ClientePJ?> ObterPorCNPJAsync(string cnpj, CancellationToken ct)
    {
        return ObterPorCnpjAsync(cnpj, ct);
    }

    public async Task<ClientePJ?> ObterPorIdAsync(Guid id, CancellationToken ct)
    {
        return await ClientesPJ
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public Task RemoveAsync(ClientePJ cliente, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(cliente);

        _db.Pessoas.Remove(cliente);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ClientePJ cliente, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(cliente);

        _db.Pessoas.Update(cliente);
        return Task.CompletedTask;
    }

    //async Task<List<ClienteListDto>> ListarAsync(CancellationToken ct)
    //{
    //    return await ClientesPJ
    //        .Select(c => new ClienteListDto(
    //            c.Id,
    //            c.Nome,
    //            c.Telefone,
    //            c.Email,
    //            "PJ",
    //            c.CNPJ,
    //            null))
    //        .ToListAsync(ct);
    //}
}