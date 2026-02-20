using GBastos.Casa_dos_Farelos.Domain.Dtos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Produtos.ObetrProdutos;

public sealed record ObterProdutosQuery(
    DateTime? DataInicio,
    DateTime? DataFim
) : IRequest<List<ProdutoDto>>;