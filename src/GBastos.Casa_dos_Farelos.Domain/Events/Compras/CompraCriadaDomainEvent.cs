using GBastos.Casa_dos_Farelos.Domain.Interfaces;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Compras;

public sealed record CompraCriadaDomainEvent(
    Guid CompraId,
    Guid FuncionarioId,
    decimal ValorTotal,
    List<CompraItemSnapshot> Itens
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}