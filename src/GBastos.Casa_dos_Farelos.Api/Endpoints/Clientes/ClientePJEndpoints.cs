using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Clientes;

public static class ClientePJEndpoints
{
    public static IEndpointRouteBuilder MapClientePJEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/clientes")
                       .WithTags("Clientes PJ")
                       .RequireAuthorization();

        group.MapGet("/pj", GetAll);
        group.MapGet("/pj/{id:guid}", GetById);

        group.MapPost("/pj", CreatePJ);

        group.MapPut("/pj/{id:guid}", UpdatePJ);

        group.MapDelete("/pj/{id:guid}", Delete);

        return app;
    }

    // ================= GET =================
    private static async Task<IResult> GetAll(AppDbContext db)
    {
        var clientes = await db.Set<Pessoa>()
            .AsNoTracking()
            .ToListAsync(); // traz para memória

        var resultado = clientes
            .Where(x => x is ClientePJ)
            .Select(x =>
            {
                var pj = (ClientePJ)x;
                return new
                {
                    pj.Id,
                    pj.Nome,
                    pj.Email,
                    pj.Telefone,
                    pj.CNPJ,
                    pj.Contato,
                    Tipo = "PJ"
                };
            });

        return Results.Ok(resultado);
    }

    private static async Task<IResult> GetById(Guid id, AppDbContext db)
    {
        var clientes = await db.Set<Pessoa>()
            .AsNoTracking()
            .ToListAsync();

        var pj = clientes
            .OfType<ClientePJ>()
            .FirstOrDefault(x => x.Id == id);

        if (pj is null)
            return Results.NotFound();

        return Results.Ok(new
        {
            pj.Id,
            pj.Nome,
            pj.Email,
            pj.Telefone,
            pj.CNPJ,
            pj.Contato,
            Tipo = "PJ"
        });
    }

    // ================= CREATE =================
    private static async Task<IResult> CreatePJ(CreateClientePJRequest request, AppDbContext db)
    {
        var cliente = new ClientePJ(
            request.Nome,
            request.Telefone,
            request.Email,
            request.Cnpj,
            request.Contato
        );

        db.Add(cliente);
        await db.SaveChangesAsync();

        return Results.Created($"/api/clientes/{cliente.Id}", new
        {
            cliente.Id,
            cliente.Nome,
            cliente.Email,
            cliente.Telefone,
            cliente.CNPJ,
            cliente.Contato,
            Tipo = "PJ"
        });
    }

    // ================= UPDATE =================
    private static async Task<IResult> UpdatePJ(Guid id, UpdateClientePJRequest request, AppDbContext db)
    {
        var cliente = await db.Set<ClientePJ>().FirstOrDefaultAsync(x => x.Id == id);
        if (cliente is null)
            return Results.NotFound();

        cliente.Atualizar(
            request.Nome,
            request.Telefone,
            request.Email,
            request.Contato,
            request.DtCadastro
        );

        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    // ================= DELETE =================
    private static async Task<IResult> Delete(Guid id, AppDbContext db)
    {
        var cliente = await db.Set<ClientePJ>().FirstOrDefaultAsync(x => x.Id == id);
        if (cliente is null)
            return Results.NotFound();

        db.Remove(cliente);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    // ================= REQUESTS =================
    public record CreateClientePJRequest(string Nome, string Telefone, string Email, string Cnpj, string Contato);
    public record UpdateClientePJRequest(string Nome, string Telefone, string Email, string Contato, DateTime DtCadastro);
}