using MediatR;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Common;

public abstract record DomainEventPG : INotification
{
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
}