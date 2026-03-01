namespace GBastos.Casa_dos_Farelos.SharedKernel.Primitives;

public abstract class StronglyTypedId<T> : IEquatable<StronglyTypedId<T>>
    where T : notnull
{
    public T Value { get; }

    protected StronglyTypedId(T value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public override bool Equals(object? obj)
    {
        return obj is StronglyTypedId<T> other && Equals(other);
    }

    public bool Equals(StronglyTypedId<T>? other)
    {
        if (other is null) return false;
        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public override int GetHashCode()
        => EqualityComparer<T>.Default.GetHashCode(Value);

    public override string ToString()
        => Value.ToString()!;
}