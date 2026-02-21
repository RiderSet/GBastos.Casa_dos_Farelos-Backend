using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Contexts;

public class PagamentoDbContext : DbContext
{
    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();

    public PagamentoDbContext(DbContextOptions<PagamentoDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PagamentoDbContext).Assembly);
    }
}