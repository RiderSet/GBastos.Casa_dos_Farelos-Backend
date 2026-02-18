using GBastos.Casa_dos_Farelos.Domain.Abstractions;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Compras;

public sealed record CompraCriadaDomainEvent(
    Guid CompraId,
    Guid FornecedorId,
    decimal ValorTotal,
    List<CompraItemSnapshot> Itens
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}