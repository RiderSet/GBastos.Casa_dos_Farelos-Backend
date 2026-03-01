using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.ComprasService.Domain.Events;

public record ProdutoCriadoEvent(
    Guid ProdutoId,
    string Nome,
    decimal PrecoVenda
) : IDomainEvent
{
    public Guid EventId => Guid.NewGuid();
    public DateTime OccurredOnUtc => DateTime.UtcNow;
}
