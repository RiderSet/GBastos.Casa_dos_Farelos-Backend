namespace GBastos.Casa_dos_Farelos.Domain.Interfaces
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        DateTime OccurredOn { get; }
    }
}
