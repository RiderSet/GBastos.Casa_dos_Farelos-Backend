namespace GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;

public record CompraItemDto(
    Guid ProdutoId,
    string NomeProduto,
    int Quantidade,
    decimal CustoUnitario,
    decimal SubTotal
);