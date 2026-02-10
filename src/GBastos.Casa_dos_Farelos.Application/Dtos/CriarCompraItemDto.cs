namespace GBastos.Casa_dos_Farelos.Application.Dtos;

public sealed record CriarCompraItemDto(
    Guid ProdutoId,
    int Quantidade,
    decimal CustoUnitario
);
