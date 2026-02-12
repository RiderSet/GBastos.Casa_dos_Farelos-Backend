namespace GBastos.Casa_dos_Farelos.Application.Dtos;

public sealed class CompraDto
{
    public Guid Id { get; init; }
    public Guid FornecedorId { get; init; }
    public DateTime DataCompra { get; init; }
    public decimal TotalCompra { get; init; }
    public List<ItemCompraDto> Itens { get; init; } = new();
}
