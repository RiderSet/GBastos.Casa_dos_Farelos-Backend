using GBastos.Casa_dos_Farelos.Domain.Interfaces;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Compras;

public sealed record CompraCriadaDomainEvent(
    Guid CompraId,
    Guid ClienteId,
    Guid FuncionarioId,
    string NomeFuncionario,
    DateTime DataCompra,
    decimal ValorTotal,
    List<CompraItemSnapshot> Itens
) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}