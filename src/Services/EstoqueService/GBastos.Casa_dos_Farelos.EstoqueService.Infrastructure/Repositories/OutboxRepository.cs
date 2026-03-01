using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Repositories;

public sealed class OutboxRepository : IOutboxRepository
{
    private readonly EstoqueDbContext _context;

    public OutboxRepository(EstoqueDbContext context)
    {
        _context = context;
    }

    public async Task<List<OutboxMessage>> GetUnprocessedAsync(
        int take,
        CancellationToken ct)
    {
        return await _context.OutboxMessages
            .Where(x =>
                x.ProcessedOnUtc == null &&
                (x.LockedUntilUtc == null ||
                 x.LockedUntilUtc < DateTime.UtcNow))
            .OrderBy(x => x.OccurredOnUtc)
            .Take(take)
            .ToListAsync(ct);
    }

    public async Task MarkAsProcessedAsync(
        Guid id,
        CancellationToken ct)
    {
        var message = await _context.OutboxMessages
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (message is null)
            return;

        message.MarkAsProcessed();
    }

    public async Task MarkAsFailedAsync(
        Guid id,
        string error,
        CancellationToken ct)
    {
        var message = await _context.OutboxMessages
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (message is null)
            return;

        message.MarkFailed(error);
    }

    public async Task AddAsync<T>(
        T @event,
        CancellationToken ct)
        where T : IIntegrationEvent
    {
        var message = OutboxMessage.CreateIntegrationEvent(@event);

        await _context.OutboxMessages.AddAsync(message, ct);
    }

    public async Task<List<OutboxMessage>> GetPendingAsync(
        int take,
        CancellationToken ct)
    {
        return await _context.OutboxMessages
            .Where(x =>
                x.ProcessedOnUtc == null &&
                (x.LockedUntilUtc == null ||
                 x.LockedUntilUtc < DateTime.UtcNow))
            .OrderBy(x => x.OccurredOnUtc)
            .Take(take)
            .ToListAsync(ct);
    }
}