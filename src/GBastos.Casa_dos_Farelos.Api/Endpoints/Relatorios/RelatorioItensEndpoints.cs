using GBastos.Casa_dos_Farelos.Application.Dtos;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Relatorios
{
    public static class RelatorioItensEndpoints
    {
        public static IEndpointRouteBuilder MapRelatorioItensEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/relatorios/itens")
                           .WithTags("Relatórios - Itens")
                           .RequireAuthorization("Gerente");

            group.MapGet("/compras", ItensComprados);
            group.MapGet("/pedidos", ItensPedidos);
            group.MapGet("/vendas", ItensVendidos);

            group.MapGet("/ranking-compras", RankingCompras);
            group.MapGet("/ranking-vendas", RankingVendas);
            group.MapGet("/ranking-pedidos", RankingPedidos);
        //  group.MapGet("/total-comprado-cliente", TotalCompradoCliente);
            group.MapGet("/funcionarios-mais-vendem", FuncionariosMaisVendem);
        //  group.MapGet("/fornecedores-por-produtos", FornecedoresPorProduto);

            return app;
        }

        // ================= COMPRAS =================

        private static async Task<IResult> ItensComprados(AppDbContext db)
        {
            var dados = await db.ItensCompra
                .AsNoTracking()
                .Select(i => new
                {
                    i.ProdutoId,
                    Produto = i.Produto.Nome,
                    i.Quantidade,
                    i.CustoUnitario,
                    i.SubTotal
                })
                .ToListAsync();

            var total = dados.Sum(x => x.SubTotal);

            return Results.Ok(new { total, dados });
        }

        // ================= PEDIDOS =================

        private static async Task<IResult> ItensPedidos(AppDbContext db)
        {
            var dados = await db.ItensPedido
                .AsNoTracking()
                .Select(i => new
                {
                    i.ProdutoId,
                    Produto = i.Produto.Nome,
                    i.Quantidade,
                    i.PrecoUnitario,
                    i.SubTotal
                })
                .ToListAsync();

            var total = dados.Sum(x => x.SubTotal);

            return Results.Ok(new { total, dados });
        }

        // ================= VENDAS =================

        private static async Task<IResult> ItensVendidos(AppDbContext db)
        {
            var dados = await db.ItensVenda
                .AsNoTracking()
                .Select(i => new
                {
                    i.ProdutoId,
                    Produto = i.DescricaoProduto,
                    i.Quantidade,
                    i.PrecoUnitario,
                    i.SubTotal
                })
                .ToListAsync();

            var total = dados.Sum(x => x.SubTotal);

            return Results.Ok(new { total, dados });
        }

        // ================= RANKINGS =================

        private static async Task<IResult> RankingCompras(AppDbContext db)
        {
            var ranking = await db.ItensCompra
                .GroupBy(i => new { i.ProdutoId, i.Produto.Nome })
                .Select(g => new
                {
                    Produto = g.Key.Nome,
                    QuantidadeComprada = g.Sum(x => x.Quantidade),
                    TotalGasto = g.Sum(x => x.SubTotal)
                })
                .OrderByDescending(x => x.QuantidadeComprada)
                .ToListAsync();

            return Results.Ok(ranking);
        }

        private static async Task<IResult> RankingPedidos(AppDbContext db)
        {
            var ranking = await db.ItensPedido
                .GroupBy(i => new { i.ProdutoId, i.Produto.Nome })
                .Select(g => new
                {
                    Produto = g.Key.Nome,
                    QuantidadePedida = g.Sum(x => x.Quantidade),
                    ValorPrevisto = g.Sum(x => x.SubTotal)
                })
                .OrderByDescending(x => x.QuantidadePedida)
                .ToListAsync();

            return Results.Ok(ranking);
        }

        private static async Task<IResult> RankingVendas(AppDbContext db)
        {
            var ranking = await db.ItensVenda
                .GroupBy(i => new { i.ProdutoId, i.DescricaoProduto })
                .Select(g => new
                {
                    Produto = g.Key.DescricaoProduto,
                    QuantidadeVendida = g.Sum(x => x.Quantidade),
                    Receita = g.Sum(x => x.SubTotal)
                })
                .OrderByDescending(x => x.QuantidadeVendida)
                .ToListAsync();

            return Results.Ok(ranking);
        }

        private static async Task<IResult> FuncionariosMaisVendem(AppDbContext db)
        {
            var ranking = await db.Vendas
                .AsNoTracking()
                .GroupBy(v => new { v.FuncionarioId, v.Funcionario.Nome })
                .Select(g => new FuncionarioMaisVendeuDto
                {
                    FuncionarioId = g.Key.FuncionarioId,
                    Nome = g.Key.Nome,
                    TotalVendido = g.Sum(x => x.TotalVenda),
                    QuantidadeVendas = g.Count()
                })
                .OrderByDescending(x => x.TotalVendido)
                .Take(10)
                .ToListAsync();

            return Results.Ok(ranking);
        }
    }
}
