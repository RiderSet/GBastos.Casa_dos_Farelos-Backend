using Dapper;
using GBastos.Casa_dos_Farelos.Application.Dtos;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Vendas.ObterVendas.Handlers;

public sealed class ObterVendasQueryHandler
    : IRequestHandler<ObterVendasQuery, List<VendaDto>>
{
    private readonly IConfiguration _config;

    public ObterVendasQueryHandler(IConfiguration config)
    {
        _config = config;
    }

    public async Task<List<VendaDto>> Handle(
        ObterVendasQuery request,
        CancellationToken cancellationToken)
    {
        await using var conn = new SqlConnection(
            _config.GetConnectionString("DefaultConn"));

        var sql = @"
SELECT
    v.Id,
    v.Data,
    p.Nome AS Cliente,
    i.ProdutoId,
    i.DescricaoProduto,
    i.Quantidade,
    i.PrecoUnitario
FROM Vendas v
LEFT JOIN Pessoas p ON p.Id = v.ClienteId
LEFT JOIN ItemVendas i ON i.VendaId = v.Id
WHERE (@DataInicio IS NULL OR v.Data >= @DataInicio)
  AND (@DataFim IS NULL OR v.Data <= @DataFim)
ORDER BY v.Data DESC
";

        var lookup = new Dictionary<Guid, VendaDto>();

        var result = await conn.QueryAsync<VendaDto, VendaItemDto, VendaDto>(
            sql,
            (venda, item) =>
            {
                if (!lookup.TryGetValue(venda.Id, out var v))
                {
                    v = new VendaDto
                    {
                        Id = venda.Id,
                        Data = venda.Data,
                        Cliente = venda.Cliente,
                        Itens = new List<VendaItemDto>()
                    };
                    lookup.Add(v.Id, v);
                }

                if (item != null)
                {
                    v.Itens.Add(new VendaItemDto
                    {
                        ProdutoId = item.ProdutoId,
                        Produto = item.Produto,
                        Quantidade = item.Quantidade,
                        PrecoUnitario = item.PrecoUnitario,
                        Total = item.Quantidade * item.PrecoUnitario
                    });
                }


                return v;
            },
            new { request.DataInicio, request.DataFim },
            splitOn: "ProdutoId"
        );

        return lookup.Values.ToList();
    }
}
