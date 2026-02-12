using GBastos.Casa_dos_Farelos.Application.Dtos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Relatorios;

public record ProdutosMaisVendidosQuery(int Top = 10)
    : IRequest<List<RankingProdutoDto>>;