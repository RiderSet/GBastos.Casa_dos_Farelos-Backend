using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxService : IOutbox
{
    private readonly AppDbContext _db;
    private readonly IIntegrationEventMapper _mapper;

    public OutboxService(AppDbContext db, IIntegrationEventMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task AddAsync(IIntegrationEvent integrationEvent, CancellationToken ct = default)
    {
        var message = OutboxMessage.CreateIntegrationEvent(integrationEvent);
        await _db.Set<OutboxMessage>().AddAsync(message, ct);
    }

    public async Task SaveEventsAsync(DbContext dbContext, CancellationToken ct)
    {
        var aggregates = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .ToList();

        foreach (var entry in aggregates)
        {
            var domainEvents = entry.Entity.DomainEvents.ToList();

            entry.Entity.ClearDomainEvents(); // 🔥 ESSENCIAL

            foreach (var domainEvent in domainEvents)
            {
                var integrationEvent = _mapper.Map(domainEvent);

                if (integrationEvent is null)
                    continue;

                var message = OutboxMessage.CreateIntegrationEvent(integrationEvent);

                await dbContext.Set<OutboxMessage>().AddAsync(message, ct);
            }
        }
    }
}