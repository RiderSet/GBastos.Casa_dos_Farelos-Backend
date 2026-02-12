using Dapper;
using GBastos.Casa_dos_Farelos.Application.Dtos;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Relatorios.Handlers;

public sealed class FuncionariosMaisVendemHandler
    : IRequestHandler<FuncionariosMaisVendemQuery, List<RankingFuncionarioDto>>
{
    private readonly IDbConnectionFactory _factory;

    public FuncionariosMaisVendemHandler(IDbConnectionFactory factory)
        => _factory = factory;

    public async Task<List<RankingFuncionarioDto>> Handle(FuncionariosMaisVendemQuery request, CancellationToken ct)
    {
        using var conn = _factory.CreateConnection();

        var sql = """
        SELECT TOP (@Top)
            v.FuncionarioId,
            f.Nome,
            COUNT(*) QuantidadeVendas,
            SUM(v.TotalVenda) TotalVendido
        FROM Vendas v
        INNER JOIN Funcionarios f ON f.Id = v.FuncionarioId
        GROUP BY v.FuncionarioId, f.Nome
        ORDER BY TotalVendido DESC
        """;

        var result = await conn.QueryAsync<RankingFuncionarioDto>(sql, request);
        return result.ToList();
    }
}