namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Common;

public abstract class AggregateRoot : BaseEntity
{
 // public Guid Id { get; protected set; }

    protected AggregateRoot()
    {
        Id = Guid.NewGuid();
    }

    protected AggregateRoot(Guid id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not AggregateRoot other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }

    public static bool operator ==(AggregateRoot? left, AggregateRoot? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(AggregateRoot? left, AggregateRoot? right)
    {
        return !Equals(left, right);
    }
}