namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Entities;

public sealed class InboxMessage
{
    public Guid Id { get; set; }
    public string EventId { get; set; } = default!;
    public DateTime ReceivedOnUtc { get; set; }
}