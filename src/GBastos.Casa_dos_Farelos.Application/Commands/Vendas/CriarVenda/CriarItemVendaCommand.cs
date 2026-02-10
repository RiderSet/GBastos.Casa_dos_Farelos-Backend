namespace GBastos.Casa_dos_Farelos.Application.Commands.Vendas.CriarVenda;

/// <summary>
/// Item da venda enviado pelo usuário
/// </summary>
public sealed record CriarItemVendaCommand(
    Guid ProdutoId,
    int Quantidade
);
