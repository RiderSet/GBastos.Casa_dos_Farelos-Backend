using MediatR;

namespace GBastos.Casa_dos_Farelos.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    DateTime OccurredOnUtc { get; }
}