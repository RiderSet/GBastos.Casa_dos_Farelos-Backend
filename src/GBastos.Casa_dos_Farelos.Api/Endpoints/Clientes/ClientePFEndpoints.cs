using Azure.Core;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Clientes;

public static class ClientePFEndpoints
{
    public static IEndpointRouteBuilder MapClientePFEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/clientes")
                       .WithTags("Clientes")
                       .RequireAuthorization();

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);

        group.MapPost("/pf", CreatePF);

        group.MapPut("/pf/{id:guid}", UpdatePF);

        group.MapDelete("/{id:guid}", Remove);

        return app;
    }

    // ================= GET =================
    private static async Task<IResult> GetAll(AppDbContext db)
    {
        var clientes = await db.Set<Pessoa>()
            .AsNoTracking()
            .ToListAsync(); // traz para memória

        var resultado = clientes.Select(x => new
        {
            x.Id,
            x.Nome,
            x.Email,
            Tipo = x.GetType().Name,
            CPF = x is ClientePF pf ? pf.CPF : null
        });

        return Results.Ok(resultado); // <-- retornar 'resultado', não 'clientes'
    }

    private static async Task<IResult> GetById(Guid id, AppDbContext db)
    {
        var cliente = await db.Set<Pessoa>()
            .AsNoTracking()
            .ToListAsync(); // traz para memória

        var resultado = cliente
            .Where(x => x.Id == id)
            .Select(x => new
            {
                x.Id,
                x.Nome,
                x.Email,
                Tipo = x.GetType().Name,
                CPF = x is ClientePF pf ? pf.CPF : null
            })
            .FirstOrDefault();

        return resultado is null ? Results.NotFound() : Results.Ok(resultado);
    }

    // ================= CREATE =================
    private static async Task<IResult> CreatePF(CreateClientePFRequest request, AppDbContext db)
    {
        var cliente = new ClientePF(
            request.Nome,
            request.Telefone,
            request.Email,
            request.Cpf,
            request.DtCadastro
        );

        db.Add(cliente);
        await db.SaveChangesAsync();

        return Results.Created($"/api/clientes/{cliente.Id}", cliente.Id);
    }

    // ================= UPDATE =================
    private static async Task<IResult> UpdatePF(Guid id, UpdateClientePFRequest request, AppDbContext db)
    {
        var cliente = await db.Set<ClientePF>().FirstOrDefaultAsync(x => x.Id == id);
        if (cliente is null) return Results.NotFound();

        cliente.Atualizar(
            request.Nome,
            request.Email,
            request.Telefone,
            request.Cpf,
            request.DtCadastro
        );

        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    // ================= DELETE =================
    private static async Task<IResult> Remove(Guid id, AppDbContext db)
    {
        // Tenta localizar a pessoa pelo Id
        var cliente = await db.Set<Pessoa>().FirstOrDefaultAsync(x => x.Id == id);

        if (cliente is null)
            return Results.NotFound(); // Retorna 404 se não existir

        // Remove a entidade do DbContext
        db.Remove(cliente);

        // Salva as alterações no banco
        await db.SaveChangesAsync();

        return Results.NoContent(); // 204 - operação concluída sem conteúdo
    }

    // ================= REQUESTS =================
    public record CreateClientePFRequest(string Nome, string Telefone, string Email, string Cpf, DateTime DtCadastro);
    public record UpdateClientePFRequest(string Nome, string Telefone, string Email, string Cpf, DateTime DtCadastro);
}