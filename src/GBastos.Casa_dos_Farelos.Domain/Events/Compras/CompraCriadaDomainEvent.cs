using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Compras;

public sealed class CompraCriadaDomainEvent : DomainEvent
{
    public Guid CompraId { get; }
    public Guid FornecedorId { get; }
    public decimal Total { get; }
    public IReadOnlyCollection<CompraItemSnapshot> Itens { get; }
    public decimal TotalCompra { get; set; }

    public CompraCriadaDomainEvent(
        Guid compraId,
        Guid fornecedorId,
        decimal TotalCompra,
        IReadOnlyCollection<CompraItemSnapshot> itens)
    {
        CompraId = compraId;
        FornecedorId = fornecedorId;
        Total = TotalCompra;
        Itens = itens;
    }
}