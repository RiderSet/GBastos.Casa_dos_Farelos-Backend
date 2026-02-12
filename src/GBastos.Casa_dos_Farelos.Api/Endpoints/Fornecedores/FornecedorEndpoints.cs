using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Fornecedores;

public static class FornecedorEndpoints
{
    public static IEndpointRouteBuilder MapFornecedorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/fornecedores")
                       .WithTags("Fornecedores")
                       .RequireAuthorization();

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);

        group.MapPost("/", Create);

        group.MapPut("/{id:guid}", Update);

        group.MapDelete("/{id:guid}", Delete);

        return app;
    }

    // ================= GET =================

    private static async Task<IResult> GetAll(AppDbContext db)
    {
        var fornecedores = await db.Set<Fornecedor>()
            .AsNoTracking()
            .Select(x => new
            {
                x.Id,
                x.Nome,
                x.Email,
                x.CNPJ,
                x.DtCadastro
            })
            .ToListAsync();

        return Results.Ok(fornecedores);
    }

    private static async Task<IResult> GetById(Guid id, AppDbContext db)
    {
        var fornecedor = await db.Set<Fornecedor>()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new
            {
                x.Id,
                x.Nome,
                x.Email,
                x.CNPJ,
                x.DtCadastro
            })
            .FirstOrDefaultAsync();

        return fornecedor is null ? Results.NotFound() : Results.Ok(fornecedor);
    }

    // ================= CREATE =================

    private static async Task<IResult> Create(CreateFornecedorRequest request, AppDbContext db)
    {
        var fornecedor = new Fornecedor(request.Nome, request.Email, request.Email, request.Documento, request.DtCadastro);

        db.Add(fornecedor);
        await db.SaveChangesAsync();

        return Results.Created($"/api/fornecedores/{fornecedor.Id}", fornecedor.Id);
    }

    // ================= UPDATE =================

    private static async Task<IResult> Update(Guid id, UpdateFornecedorRequest request, AppDbContext db)
    {
        var fornecedor = await db.Set<Fornecedor>().FirstOrDefaultAsync(x => x.Id == id);
        if (fornecedor is null)
            return Results.NotFound();

        fornecedor.Atualizar(request.Nome, request.Email, request.Documento, request.DtCadastro);

        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    // ================= DELETE =================

    private static async Task<IResult> Delete(Guid id, AppDbContext db)
    {
        var fornecedor = await db.Set<Fornecedor>().FirstOrDefaultAsync(x => x.Id == id);
        if (fornecedor is null)
            return Results.NotFound();

        db.Remove(fornecedor);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    // ================= REQUESTS =================

    public record CreateFornecedorRequest(string Nome, string Email, string Documento, DateTime DtCadastro);
    public record UpdateFornecedorRequest(string Nome, string Email, string Documento, DateTime DtCadastro);
}