using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Clientes;

public static class ClienteEndpoints
{
    public static IEndpointRouteBuilder MapClienteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/clientes")
                       .WithTags("Clientes")
                       .RequireAuthorization();

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);

        group.MapPost("/pf", CreatePF);
        group.MapPost("/pj", CreatePJ);

        group.MapPut("/pf/{id:guid}", UpdatePF);
        group.MapPut("/pj/{id:guid}", UpdatePJ);

        group.MapDelete("/{id:guid}", Delete);

        return app;
    }

    // ================= GET =================

    private static async Task<IResult> GetAll(AppDbContext db)
    {
        var clientes = await db.Set<Pessoa>()
            .AsNoTracking()
            .Select(x => new
            {
                x.Id,
                x.Nome,
                x.Email,
                Documento = x.Documento
            })
            .ToListAsync();

        return Results.Ok(clientes);
    }

    private static async Task<IResult> GetById(Guid id, AppDbContext db)
    {
        var cliente = await db.Set<Pessoa>()
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new
            {
                x.Id,
                x.Nome,
                x.Email,
                Documento = x.Documento
            })
            .FirstOrDefaultAsync();

        return cliente is null ? Results.NotFound() : Results.Ok(cliente);
    }

    // ================= CREATE =================

    private static async Task<IResult> CreatePF(CreateClientePFRequest req, AppDbContext db)
    {
        var cliente = new ClientePF(req.Nome, req.Email, req.Cpf);

        db.Add(cliente);
        await db.SaveChangesAsync();

        return Results.Created($"/api/clientes/{cliente.Id}", cliente.Id);
    }

    private static async Task<IResult> CreatePJ(CreateClientePJRequest req, AppDbContext db)
    {
        var cliente = new ClientePJ(req.RazaoSocial, req.Email, req.Cnpj);

        db.Add(cliente);
        await db.SaveChangesAsync();

        return Results.Created($"/api/clientes/{cliente.Id}", cliente.Id);
    }

    // ================= UPDATE =================

    private static async Task<IResult> UpdatePF(Guid id, UpdateClientePFRequest req, AppDbContext db)
    {
        var cliente = await db.Set<ClientePF>().FirstOrDefaultAsync(x => x.Id == id);
        if (cliente is null) return Results.NotFound();

        cliente.AtualizarDados(req.nome, req.cpf, req.telefone, req.email, req.dataNascimento);

        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> UpdatePJ(Guid id, UpdateClientePJRequest req, AppDbContext db)
    {
        var cliente = await db.Set<ClientePJ>().FirstOrDefaultAsync(x => x.Id == id);
        if (cliente is null) return Results.NotFound();

        cliente.AtualizarDados(req.RazaoSocial, req.Email);

        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    // ================= DELETE =================

    private static async Task<IResult> Delete(Guid id, AppDbContext db)
    {
        var cliente = await db.Set<Pessoa>().FirstOrDefaultAsync(x => x.Id == id);
        if (cliente is null) return Results.NotFound();

        db.Remove(cliente);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    // ================= REQUESTS =================

    public record CreateClientePFRequest(string Nome, string Email, string Cpf);
    public record CreateClientePJRequest(string RazaoSocial, string Email, string Cnpj);

    public record UpdateClientePFRequest(string nome, string cpf, string telefone, string email, DateTime dataNascimento);
    public record UpdateClientePJRequest(string RazaoSocial, string Email);
}