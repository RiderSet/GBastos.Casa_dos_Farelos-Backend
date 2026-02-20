namespace GBastos.Casa_dos_Farelos.Domain.Interfaces
{
    public interface IDomainEvent
    {
        DateTime OccurredOnUtc { get; }
    }
}
