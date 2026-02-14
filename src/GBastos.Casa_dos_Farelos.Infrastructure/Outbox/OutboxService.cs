using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
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
        var message = OutboxMessage.Create(
            integrationEvent,
            integrationEvent.Id,
            integrationEvent.OccurredOn
        );

        await _db.Set<OutboxMessage>().AddAsync(message, ct);
    }

    public async Task SaveEventsAsync(DbContext dbContext, CancellationToken ct)
    {
        var domainEvents = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            var integrationEvent = _mapper.Map(domainEvent);

            var message = OutboxMessage.Create(
                integrationEvent,
                integrationEvent.Id,
                integrationEvent.OccurredOn
            );

            await dbContext.Set<OutboxMessage>().AddAsync(message, ct);
        }
    }
}