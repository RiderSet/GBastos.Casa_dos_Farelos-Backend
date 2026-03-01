using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Persistence.Context;

public class EstoqueDbContext : DbContext
{
    public EstoqueDbContext(DbContextOptions<EstoqueDbContext> opt)
        : base(opt) { }

    public DbSet<Reserva> Reservas => Set<Reserva>();
    public DbSet<ProdutoEstoque> Produtos => Set<ProdutoEstoque>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<IdempotencyKey> IdempotencyKeys => Set<IdempotencyKey>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<OutboxMessage>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Type).IsRequired();
            b.Property(x => x.Payload).IsRequired();
            b.HasIndex(x => x.ProcessedOnUtc);
        });

        builder.Entity<IdempotencyKey>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Key).IsUnique();
            b.Property(x => x.Key).IsRequired();
        }); 
        
        builder.Entity<Reserva>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.RowVersion)
                .IsRowVersion();
            b.Property(x => x.Quantidade)
                .IsRequired();
            b.HasIndex(x => x.ProdutoId);
            b.HasIndex(x => x.ExpiraEm);
        });

        builder.Entity<OutboxMessage>(b =>
        {
            b.HasKey(x => x.Id);

            b.HasIndex(x => x.ProcessedOnUtc);
            b.HasIndex(x => x. RetryCount);

            b.Property(x => x.Type).IsRequired();
            b.Property(x => x.Payload).IsRequired();
        });

        base.OnModelCreating(builder);
    }
}