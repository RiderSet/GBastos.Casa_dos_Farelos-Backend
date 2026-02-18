using MediatR;

namespace GBastos.Casa_dos_Farelos.Domain.Interfaces;

public interface IDomainEvent : INotification
{
    DateTime OccurredOnUtc { get; }
}