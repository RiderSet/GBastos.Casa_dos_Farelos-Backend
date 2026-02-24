using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Common;
using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Entities;
using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Outbox;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.Contexts;

public sealed class PagamentoDbContext : DbContext
{
    private readonly IMediator _mediator;

    public PagamentoDbContext(
        DbContextOptions<PagamentoDbContext> options,
        IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Pagamento> Pagamentos => Set<Pagamento>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();

    public DbSet<OutboxMessagePG> OutboxMessages => Set<OutboxMessagePG>();
    public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<BaseEntity>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        ChangeTracker
            .Entries<BaseEntity>()
            .ToList()
            .ForEach(e => e.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(PagamentoDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}