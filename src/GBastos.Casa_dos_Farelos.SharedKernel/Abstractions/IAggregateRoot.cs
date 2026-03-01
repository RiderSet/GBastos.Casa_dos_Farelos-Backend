using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;

public interface IAggregateRoot : IEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}