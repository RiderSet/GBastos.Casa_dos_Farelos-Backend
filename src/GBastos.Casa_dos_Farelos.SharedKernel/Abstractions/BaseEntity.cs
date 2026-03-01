namespace GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public object GetId() => Id!;
}