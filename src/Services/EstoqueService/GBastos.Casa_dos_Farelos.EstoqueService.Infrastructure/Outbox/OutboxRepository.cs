using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Persistence.Context;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;

public sealed class OutboxRepository : IOutboxRepository
{
    private readonly EstoqueDbContext _context;

    public OutboxRepository(EstoqueDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync<T>(T @event, CancellationToken ct)
    {
        var message = new OutboxMessage(
            typeof(T).Name,
            JsonSerializer.Serialize(@event));

        await _context.OutboxMessages.AddAsync(message, ct);
    }
}