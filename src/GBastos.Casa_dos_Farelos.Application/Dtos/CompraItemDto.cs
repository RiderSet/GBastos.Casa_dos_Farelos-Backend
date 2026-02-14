namespace GBastos.Casa_dos_Farelos.Application.Dtos;

public record CompraItemDto(
    Guid ProdutoId,
    string NomeProduto,
    int Quantidade,
    decimal CustoUnitario,
    decimal SubTotal
);