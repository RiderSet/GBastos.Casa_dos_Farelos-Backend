namespace GBastos.Casa_dos_Farelos.Domain.Dtos;

public sealed class VendaItemDto
{
    public Guid ProdutoId { get; init; }
    public string Produto { get; init; } = string.Empty;
    public int Quantidade { get; init; }
    public decimal PrecoUnitario { get; init; }
    public decimal Total { get; init; }
}
