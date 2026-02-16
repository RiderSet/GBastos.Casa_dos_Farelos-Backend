using GBastos.Casa_dos_Farelos.Domain.Abstractions;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Compras;

public sealed record CompraCriadaDomainEvent(
    Guid CompraId,
    Guid FornecedorId,
    decimal ValorTotal,
    List<CompraItemSnapshot> Itens
) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

//public sealed class CompraCriadaDomainEvent : INotification
//{
//    public Guid CompraId { get; }
//    public Guid FornecedorId { get; }
//    public decimal Total { get; }
//    public IReadOnlyCollection<CompraItemSnapshot> Itens { get; }
//    public decimal TotalCompra { get; set; }

//    public CompraCriadaDomainEvent(
//        Guid compraId,
//        Guid fornecedorId,
//        decimal ValorTotal,
//        IReadOnlyCollection<CompraItemSnapshot> itens)
//    {
//        CompraId = compraId;
//        FornecedorId = fornecedorId;
//        Total = TotalCompra;
//        Itens = itens;
//    }
//}