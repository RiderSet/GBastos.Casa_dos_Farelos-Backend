using GBastos.Casa_dos_Farelos.Domain.Dtos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Produtos.ObetrProdutos;

public sealed class ObterProdutoPorIdQuery : IRequest<ProdutoDto>
{
    public Guid Id { get; init; }

    public ObterProdutoPorIdQuery(Guid id)
    {
        Id = id;
    }
}
