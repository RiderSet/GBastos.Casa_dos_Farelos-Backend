using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Inbox;

public sealed class InboxRepository : IInboxRepository
{
    private readonly PagamentoDbContext _context;

    public InboxRepository(PagamentoDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct)
        => _context.InboxMessages.AnyAsync(x => x.Id == id, ct);

    public async Task AddAsync(InboxMessage message, CancellationToken ct)
        => await _context.InboxMessages.AddAsync(message, ct);

    public Task SaveChangesAsync(CancellationToken ct)
        => _context.SaveChangesAsync(ct);
}