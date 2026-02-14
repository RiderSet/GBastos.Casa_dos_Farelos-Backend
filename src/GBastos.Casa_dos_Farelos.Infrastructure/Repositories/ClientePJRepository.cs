using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Repositories
{
    public sealed class ClientePJRepository : IClientePJRepository
    {
        private readonly AppDbContext _db;

        public ClientePJRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(ClientePJ cliente, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(cliente);

            await _db.ClientesPJ.AddAsync(cliente, ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistePorCpfAsync(string cpf, CancellationToken ct)
        {
            // Mantive o nome do método, mas é CNPJ na entidade
            return await _db.ClientesPJ
                .AsNoTracking()
                .AnyAsync(x => x.CNPJ == cpf, ct);
        }

        public async Task<List<ClientePJ>> ListarAsync(CancellationToken ct)
        {
            return await _db.ClientesPJ
                .AsNoTracking()
                .OrderBy(x => x.NomeFantasia)
                .ToListAsync(ct);
        }

        public async Task<ClientePJ?> ObterPorCpfAsync(string cpf, CancellationToken ct)
        {
            return await _db.ClientesPJ
                .FirstOrDefaultAsync(x => x.CNPJ == cpf, ct);
        }

        public async Task<ClientePJ?> ObterPorIdAsync(Guid id, CancellationToken ct)
        {
            return await _db.ClientesPJ
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task RemoveAsync(ClientePJ cliente, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(cliente);

            _db.ClientesPJ.Remove(cliente);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(ClientePJ cliente, CancellationToken ct)
        {
            ArgumentNullException.ThrowIfNull(cliente);

            _db.ClientesPJ.Update(cliente);
            await _db.SaveChangesAsync(ct);
        }
    }
}
