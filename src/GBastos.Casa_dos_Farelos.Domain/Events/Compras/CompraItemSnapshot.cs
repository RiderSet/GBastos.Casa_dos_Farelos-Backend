namespace GBastos.Casa_dos_Farelos.Domain.Events.Compras;

public sealed record CompraItemSnapshot(
    Guid ProdutoId,
    string NomeProduto,
    int Quantidade,
    decimal PrecoUnitario,
    decimal SubTotal
);