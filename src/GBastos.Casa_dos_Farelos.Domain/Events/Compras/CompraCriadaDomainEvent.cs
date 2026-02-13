using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Compras;

public sealed class CompraCriadaDomainEvent : DomainEvent
{
    public Guid CompraId { get; }
    public Guid FornecedorId { get; }
    public decimal ValorTotal { get; }
    public List<CompraItemDto> Itens { get; } = new List<CompraItemDto>();

    public CompraCriadaDomainEvent(Guid compraId, Guid fornecedorId, decimal valorTotal, List<CompraItemDto> itens)
    {
        CompraId = compraId;
        FornecedorId = fornecedorId;
        ValorTotal = valorTotal;
        Itens = itens ?? new List<CompraItemDto>();
    }
}

public sealed record CompraItemDto(
    Guid ProdutoId,
    string NomeProduto,
    int Quantidade,
    decimal PrecoUnitario,
    decimal SubTotal);