using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Persistence.Context;

public class CadastroDbContext : DbContext
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public CadastroDbContext(DbContextOptions<CadastroDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CadastroDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}