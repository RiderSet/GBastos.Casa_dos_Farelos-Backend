using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.SharedKernel.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.ORM;

public class EfRepository<T, TId> : IRepository<T, TId>
    where T : AggregateRoot<TId>
    where TId : notnull
{
    protected readonly DbContext _context;

    public EfRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(TId id)
        => await _context.Set<T>().FindAsync(id);

    public async Task AddAsync(T entity)
        => await _context.Set<T>().AddAsync(entity);

    public void Update(T entity)
        => _context.Set<T>().Update(entity);

    public void Remove(T entity)
        => _context.Set<T>().Remove(entity);
}