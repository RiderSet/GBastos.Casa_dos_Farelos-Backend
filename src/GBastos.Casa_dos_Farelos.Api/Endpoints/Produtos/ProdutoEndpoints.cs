using GBastos.Casa_dos_Farelos.Application.Queries.Produtos.ObetrProdutos;
using GBastos.Casa_dos_Farelos.Application.Queries.Relatorios;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Produtos;

public static class ProdutoEndpoints
{
    public static IEndpointRouteBuilder MapProdutoEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/produtos")
                       .WithTags("Produtos")
                       .RequireAuthorization();

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapPost("/", Create);
        group.MapPut("/{id:guid}", Update);
        group.MapPatch("/{id:guid}/estoque", AjustarEstoque);
        group.MapDelete("/{id:guid}", Delete);

        return app;
    }

    // ================= GET =================

    private static async Task<IResult> GetAll(AppDbContext db)
    {
        var produtos = await db.Set<Produto>()
            .AsNoTracking()
            .Select(p => new ProdutoResponse(
                p.Id,
                p.Nome,
                p.DescricaoProduto,
                p.PrecoVenda,
                p.CategoriaId,
                p.QuantEstoque))
            .ToListAsync();

        return Results.Ok(produtos);
    }

    private static async Task<IResult> GetById(Guid id, AppDbContext db)
    {
        var produto = await db.Set<Produto>()
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProdutoResponse(
                p.Id,
                p.Nome,
                p.DescricaoProduto,
                p.PrecoVenda,
                p.CategoriaId,
                p.QuantEstoque))
            .FirstOrDefaultAsync();

        return produto is null ? Results.NotFound() : Results.Ok(produto);
    }

    // ================= CREATE =================

    private static async Task<IResult> Create(CreateProdutoRequest req, AppDbContext db)
    {
        var produto = new Produto(req.Nome, req.Descricao, req.Preco, req.CategoriaId, req.QuantEstoque);

        db.Add(produto);
        await db.SaveChangesAsync();

        var response = new ProdutoResponse(produto.Id, produto.Nome, produto.DescricaoProduto, produto.PrecoVenda, produto.CategoriaId, produto.QuantEstoque);
        return Results.Created($"/api/produtos/{produto.Id}", response);
    }

    // ================= UPDATE =================

    private static async Task<IResult> Update(Guid id, UpdateProdutoRequest req, AppDbContext db)
    {
        var produto = await db.Set<Produto>().FirstOrDefaultAsync(p => p.Id == id);
        if (produto is null) return Results.NotFound();

        produto.Atualizar(req.Nome, req.Descricao, req.Preco, req.CategoriaId, req.QuantEstoque);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> AjustarEstoque(Guid id, AjustarEstoqueRequest req, AppDbContext db)
    {
        var produto = await db.Set<Produto>().FirstOrDefaultAsync(p => p.Id == id);
        if (produto is null) return Results.NotFound();

        produto.BaixarEstoque(req.quantEstoque);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    // ================= DELETE =================

    private static async Task<IResult> Delete(Guid id, AppDbContext db)
    {
        var produto = await db.Set<Produto>().FirstOrDefaultAsync(p => p.Id == id);
        if (produto is null) return Results.NotFound();

        db.Remove(produto);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> ProdutosMaisVendidos(IMediator mediator)
    {
        var result = await mediator.Send(new ProdutosMaisVendidosQuery());
        return Results.Ok(result);
    }

    // ================= REQUEST/RESPONSE =================

    public record CreateProdutoRequest(string Nome, string Descricao, decimal Preco, Guid CategoriaId, int QuantEstoque);
    public record UpdateProdutoRequest(string Nome, string Descricao, decimal Preco, Guid CategoriaId, int QuantEstoque);
    public record AjustarEstoqueRequest(int quantEstoque);

    public record ProdutoResponse(Guid Id, string Nome, string Descricao, decimal Preco, Guid CategoriaId, int QuantEstoque);
}