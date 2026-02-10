using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Relatorios;

public static class RelatorioEndpoints
{
    public static IEndpointRouteBuilder MapRelatorioEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/relatorios")
                       .WithTags("Relatórios")
                       .RequireAuthorization("Gerente"); // policy

        group.MapGet("/vendas-total", TotalVendas);
        group.MapGet("/compras-total", TotalCompras);
        group.MapGet("/faturamento", FaturamentoPeriodo);
        group.MapGet("/produtos-mais-vendidos", ProdutosMaisVendidos);

        return app;
    }

    // ================= TOTAL VENDAS =================

    private static async Task<IResult> TotalVendas(
        DateTime? inicio,
        DateTime? fim,
        AppDbContext db)
    {
        var query = db.Vendas.AsNoTracking();

        if (inicio.HasValue)
            query = query.Where(v => v.DataVenda >= inicio.Value);

        if (fim.HasValue)
            query = query.Where(v => v.DataVenda <= fim.Value);

        var total = await query.SumAsync(v => v.TotalVenda);

        return Results.Ok(new { total });
    }

    // ================= TOTAL COMPRAS =================

    private static async Task<IResult> TotalCompras(
        DateTime? inicio,
        DateTime? fim,
        AppDbContext db)
    {
        var query = db.Compras.AsNoTracking();

        if (inicio.HasValue)
            query = query.Where(c => c.DataCompra >= inicio.Value);

        if (fim.HasValue)
            query = query.Where(c => c.DataCompra <= fim.Value);

        var total = await query.SumAsync(c => c.TotalCompra);

        return Results.Ok(new { total });
    }

    // ================= FATURAMENTO =================

    private static async Task<IResult> FaturamentoPeriodo(
        DateTime inicio,
        DateTime fim,
        AppDbContext db)
    {
        var totalVendas = await db.Vendas
            .Where(v => v.DataVenda >= inicio && v.DataVenda <= fim)
            .SumAsync(v => v.TotalVenda);

        var totalCompras = await db.Compras
            .Where(c => c.DataCompra >= inicio && c.DataCompra <= fim)
            .SumAsync(c => c.TotalCompra);

        var lucro = totalVendas - totalCompras;

        return Results.Ok(new
        {
            totalVendas,
            totalCompras,
            lucro
        });
    }

    // ================= PRODUTOS MAIS VENDIDOS =================

    private static async Task<IResult> ProdutosMaisVendidos(AppDbContext db)
    {
        var produtos = await db.ItensVenda
            .AsNoTracking()
            .GroupBy(i => new { i.ProdutoId, i.Produto.Nome })
            .Select(g => new
            {
                ProdutoId = g.Key.ProdutoId,
                Nome = g.Key.Nome,
                QuantidadeVendida = g.Sum(x => x.Quantidade)
            })
            .OrderByDescending(x => x.QuantidadeVendida)
            .Take(10)
            .ToListAsync();

        return Results.Ok(produtos);
    }
}
