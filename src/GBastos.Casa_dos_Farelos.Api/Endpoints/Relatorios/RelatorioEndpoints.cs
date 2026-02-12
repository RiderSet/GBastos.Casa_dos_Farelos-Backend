using GBastos.Casa_dos_Farelos.Application.Queries.Relatorios;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using MediatR;
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
        group.MapGet("/funcionarios-mais-vendem", FuncionariosMaisVendem);
        group.MapGet("/total-comprado-cliente/{clienteId:guid}", TotalCompradoCliente);
        group.MapGet("/fornecedores-por-produto/{produtoId:guid}", FornecedoresPorProduto);

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

        var total = await query
            .SelectMany(c => c.Itens)
            .SumAsync(i => i.SubTotal);

        return Results.Ok(new { total });
    }

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

    private static async Task<IResult> FuncionariosMaisVendem(AppDbContext db)
    {
        var funcionarios = await db.Vendas
            .AsNoTracking()
            .GroupBy(v => new { v.FuncionarioId, v.Funcionario.Nome })
            .Select(g => new
            {
                FuncionarioId = g.Key.FuncionarioId,
                Nome = g.Key.Nome,
                QuantidadeVendas = g.Count(),
                TotalVendido = g.Sum(v => v.TotalVenda)
            })
            .OrderByDescending(x => x.TotalVendido)
            .Take(10)
            .ToListAsync();

        return Results.Ok(funcionarios);
    }

    private static async Task<IResult> TotalCompradoCliente(
    Guid clienteId,
    DateTime? inicio,
    DateTime? fim,
    AppDbContext db)
    {
        var query = db.Vendas
            .AsNoTracking()
            .Where(v => v.ClienteId == clienteId);

        if (inicio.HasValue)
            query = query.Where(v => v.DataVenda >= inicio.Value);

        if (fim.HasValue)
            query = query.Where(v => v.DataVenda <= fim.Value);

        var total = await query.SumAsync(v => v.TotalVenda);

        return Results.Ok(new
        {
            clienteId,
            total
        });
    }

    private static async Task<IResult> FornecedoresPorProduto(
    Guid produtoId,
    AppDbContext db)
    {
        var fornecedores = await db.Compras
            .AsNoTracking()
            .Where(c => c.Itens.Any(i => i.ProdutoId == produtoId))
            .Select(c => new
            {
                c.FornecedorId,
                c.Fornecedor.Nome
            })
            .Distinct()
            .ToListAsync();

        return Results.Ok(fornecedores);
    }

    private static async Task<IResult> FaturamentoPeriodo(
    DateTime inicio,
    DateTime fim,
    IMediator mediator)
    {
        var result = await mediator.Send(new FaturamentoPeriodoQuery(inicio, fim));
        return Results.Ok(result);
    }

}
