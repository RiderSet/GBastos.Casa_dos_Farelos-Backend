namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;

public sealed class IdempotencyKey
{
    public IdempotencyKey(string key)
    {
        Key = key;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Key { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}