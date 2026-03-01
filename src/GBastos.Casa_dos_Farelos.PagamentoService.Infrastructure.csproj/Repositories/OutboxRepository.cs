using GBastos.Casa_dos_Farelos.PagamentoService.Api.Interfaces;
using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Repositories;

public sealed class OutboxRepository : IOutboxRepository
{
    private readonly PGServiceContext _context;

    public OutboxRepository(PGServiceContext context)
    {
        _context = context;
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

        await _context.SaveChangesAsync(ct);
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

        await _context.SaveChangesAsync(ct);
    }

    public async Task AddAsync<T>(
        T @event,
        CancellationToken ct)
    {
        OutboxMessagePG message;

        if (@event is IDomainEvent domainEvent)
        {
            message = OutboxMessagePG.Create(domainEvent);
        }
        else
        {
            message = OutboxMessagePG.CreateIntegrationEvent(@event!);
        }

        await _context.OutboxMessages.AddAsync(message, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<List<OutboxMessagePG>> GetPendingAsync(
        int take,
        CancellationToken ct)
    {
        return await _context.OutboxMessages
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(take)
            .ToListAsync(ct);
    }
}