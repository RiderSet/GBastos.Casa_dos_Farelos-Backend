using GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;

namespace GBastos.Casa_dos_Farelos.Domain.Dtos;

public sealed class CompraDetalhadaDto
{
    public Guid Id { get; init; }
    public DateTime DataCompra { get; init; }
    public Guid FornecedorId { get; init; }
    public decimal Total { get; init; }

    public List<ItemCompraDto> Itens { get; init; } = new();
}
