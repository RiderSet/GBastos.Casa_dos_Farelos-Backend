namespace GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;

public sealed record ItemCompraDto(
    Guid ProdutoId,
    string NomeProduto,
    int Quantidade,
    decimal PrecoUnitario
)
{
    public decimal SubTotal => Quantidade * PrecoUnitario;
}