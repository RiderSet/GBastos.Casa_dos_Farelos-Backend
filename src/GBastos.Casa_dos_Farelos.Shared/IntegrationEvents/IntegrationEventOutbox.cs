using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

public sealed class IntegrationEventOutbox : IIntegrationEventOutbox
{
    private readonly AppDbContext _db;

    public IntegrationEventOutbox(AppDbContext db)
        => _db = db;

    public Task AddAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(IIntegrationEvent integrationEvent, CancellationToken ct)
    {
        var msg = new IntegrationOutboxMessage
        {
            Id = integrationEvent.Id,
            OccurredOnUtc = integrationEvent.OccurredOnUtc,
            Type = integrationEvent.GetType().AssemblyQualifiedName!,
            Content = JsonSerializer.Serialize(integrationEvent)
        };

        _db.Set<IntegrationOutboxMessage>().Add(msg);

        return Task.CompletedTask;
    }
}