using MediatR;

namespace GBastos.Casa_dos_Farelos.Domain.Interfaces;

public interface IDomainEvent : INotification
{
<<<<<<< HEAD
    public interface IDomainEvent
    {
        DateTime OccurredOnUtc { get; }
    }
}
=======
    DateTime OccurredOnUtc { get; }
}
>>>>>>> 532a5516c5422679921d3b0f6d7a9995a5d30bda
