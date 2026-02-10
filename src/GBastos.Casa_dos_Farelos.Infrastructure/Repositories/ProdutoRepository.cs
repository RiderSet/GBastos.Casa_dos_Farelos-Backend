using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _db;

        public ProdutoRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Produto produto, CancellationToken ct)
        {
            if (produto == null) throw new ArgumentNullException(nameof(produto));
            await _db.Produtos.AddAsync(produto, ct);
        }

        public async Task<bool> ExistePorNomeAsync(string nome, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return false;

            return await _db.Produtos
                .AsNoTracking()
                .AnyAsync(p => p.Nome == nome, ct);
        }

        public async Task<Produto?> ObterPorIdAsync(Guid id, CancellationToken ct)
        {
            return await _db.Produtos
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public Task RemoveAsync(Produto produto, CancellationToken ct)
        {
            if (produto == null) throw new ArgumentNullException(nameof(produto));

            _db.Produtos.Remove(produto);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Produto produto, CancellationToken ct)
        {
            if (produto == null) throw new ArgumentNullException(nameof(produto));

            // EF já rastreia automaticamente, mas garantimos explicitamente
            _db.Produtos.Update(produto);
            return Task.CompletedTask;
        }

        async Task<Dictionary<Guid, Produto>> IProdutoRepository.ObterPorIdsAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return await _db.Produtos
                .Where(p => ids.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, ct);
        }
    }
}