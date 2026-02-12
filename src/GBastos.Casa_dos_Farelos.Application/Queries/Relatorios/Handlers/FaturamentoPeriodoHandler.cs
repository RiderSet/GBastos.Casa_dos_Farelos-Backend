using Dapper;
using GBastos.Casa_dos_Farelos.Application.Dtos;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Relatorios.Handlers;

public sealed class FaturamentoPeriodoHandler
    : IRequestHandler<FaturamentoPeriodoQuery, FaturamentoDto>
{
    private readonly IDbConnectionFactory _factory;

    public FaturamentoPeriodoHandler(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<FaturamentoDto> Handle(FaturamentoPeriodoQuery request, CancellationToken ct)
    {
        using var conn = _factory.CreateConnection();

        var sql = """
        SELECT
            (SELECT ISNULL(SUM(TotalVenda),0) FROM Vendas WHERE DataVenda BETWEEN @Inicio AND @Fim) AS TotalVendas,
            (SELECT ISNULL(SUM(TotalCompra),0) FROM Compras WHERE DataCompra BETWEEN @Inicio AND @Fim) AS TotalCompras
        """;

        var result = await conn.QuerySingleAsync(sql, request);

        decimal lucro = result.TotalVendas - result.TotalCompras;

        return new(result.TotalVendas, result.TotalCompras, lucro);
    }
}