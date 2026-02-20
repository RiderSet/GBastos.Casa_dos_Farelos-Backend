using GBastos.Casa_dos_Farelos.Application.Common;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class OutboxRepository : IOutboxRepository
{
    private readonly ApplicationDbContext _context;

    public OutboxRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(IIntegrationEvent @event, CancellationToken ct)
    {
        var message = new OutboxMessage(
            Guid.NewGuid(),
            @event.GetType().FullName!,
            JsonSerializer.Serialize(@event),
            DateTime.UtcNow
        );
        await _context.OutboxMessages.AddAsync(message, ct);
    }

    public async Task<IReadOnlyList<OutboxItem>> GetPendingAsync(int batchSize, CancellationToken ct)
    {
        var messages = await _context.OutboxMessages
            .AsNoTracking()
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(batchSize)
            .Select(x => new OutboxItem
            {
                Id = x.Id,
                OccurredOn = x.OccurredOnUtc,
                Type = x.EventName,
                Payload = x.Payload
            })
            .ToListAsync(ct);

        return messages;
    }

    public async Task MarkAsProcessedAsync(Guid id, CancellationToken ct)
    {
        var message = await _context.OutboxMessages
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (message is null)
            return;

        message.MarkProcessed();
    }

    public async Task MarkAsFailedAsync(Guid id, string error, CancellationToken ct)
    {
        var message = await _context.OutboxMessages
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (message is null)
            return;

        message.MarkFailed(error);
    }
}