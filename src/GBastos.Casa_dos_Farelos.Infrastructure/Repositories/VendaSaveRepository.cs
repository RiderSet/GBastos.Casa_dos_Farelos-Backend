using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Repositories
{
    public class VendaSaveRepository : IVendaSaveRepository
    {
        private readonly AppDbContext _db;

        public VendaSaveRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Venda venda, CancellationToken ct)
        {
            await _db.Vendas.AddAsync(venda, ct);
        }

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);
    }
}
