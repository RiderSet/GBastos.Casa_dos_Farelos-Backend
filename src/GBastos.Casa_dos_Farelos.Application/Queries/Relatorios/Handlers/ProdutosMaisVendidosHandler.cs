using Dapper;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Dtos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Relatorios.Handlers;

public sealed class ProdutosMaisVendidosHandler
    : IRequestHandler<ProdutosMaisVendidosQuery, List<RankingProdutoDto>>
{
    private readonly IDbConnectionFactory _factory;

    public ProdutosMaisVendidosHandler(IDbConnectionFactory factory)
        => _factory = factory;

    public async Task<List<RankingProdutoDto>> Handle(ProdutosMaisVendidosQuery request, CancellationToken ct)
    {
        using var conn = _factory.CreateConnection();

        var sql = """
        SELECT TOP (@Top)
            iv.ProdutoId,
            p.Nome,
            SUM(iv.Quantidade) QuantidadeVendida
        FROM ItensVenda iv
        INNER JOIN Produtos p ON p.Id = iv.ProdutoId
        GROUP BY iv.ProdutoId, p.Nome
        ORDER BY QuantidadeVendida DESC
        """;

        var result = await conn.QueryAsync<RankingProdutoDto>(sql, request);
        return result.ToList();
    }
}