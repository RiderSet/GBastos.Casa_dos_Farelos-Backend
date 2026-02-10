using Dapper;
using GBastos.Casa_dos_Farelos.Application.Dtos;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using System.Data;

namespace GBastos.Casa_dos_Farelos.Infrastructure.ReadModels.Relatorios;

public sealed class RelatorioVendasQueryService : IRelatorioVendasQueryService
{
    private readonly IDbConnection _conn;
    private readonly ICacheService _cache;

    public RelatorioVendasQueryService(IDbConnection conn, ICacheService cache)
    {
        _conn = conn;
        _cache = cache;
    }

    public async Task<IEnumerable<RelatorioDto>> VendasPeriodo(DateTime inicio, DateTime fim)
    {
        var cacheKey = $"relatorio:{inicio:yyyyMMdd}:{fim:yyyyMMdd}";
        var cache = await _cache.GetAsync<IEnumerable<RelatorioDto>>(cacheKey);

        if (cache != null)
            return cache;

        var sql = """
            SELECT CAST(Data AS DATE) Dia, SUM(Total) Total
            FROM Vendas
            WHERE Data BETWEEN @inicio AND @fim
            GROUP BY CAST(Data AS DATE)
            ORDER BY Dia
        """;

        var data = await _conn.QueryAsync<RelatorioDto>(sql, new { inicio, fim });

        await _cache.SetAsync(cacheKey, data, TimeSpan.FromMinutes(2));

        return data;
    }
}