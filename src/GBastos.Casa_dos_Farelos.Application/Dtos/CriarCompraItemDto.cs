namespace GBastos.Casa_dos_Farelos.Application.Dtos;

public sealed record CriarCompraItemDto(
    Guid ProdutoId,
    string NomeProduto,
    int Quantidade,
    decimal CustoUnitario
);
