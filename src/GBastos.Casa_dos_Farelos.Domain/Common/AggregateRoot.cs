using System.ComponentModel.DataAnnotations;

namespace GBastos.Casa_dos_Farelos.Domain.Common;

public abstract class AggregateRoot : BaseEntity
{
    [Timestamp]
    public byte[] Version { get; private set; } = Array.Empty<byte>();
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }

    protected void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public abstract void ValidateInvariants();
}