using GBastos.Casa_dos_Farelos.Application.Queries.Relatorios;
using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Domain.Enums;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Funcionarios;

public static class FuncionarioEndpoints
{
    public static IEndpointRouteBuilder MapFuncionarioEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/funcionarios")
                       .WithTags("Funcionarios")
                       .RequireAuthorization();

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById);
        group.MapPost("/", Create);
        group.MapPut("/{id:guid}", Update);
        group.MapDelete("/{id:guid}", Delete);

        return app;
    }

    // ================= GET ALL =================

    private static async Task<IResult> GetAll(AppDbContext db)
    {
        var funcionarios = await db.Funcionarios
            .AsNoTracking()
            .Select(x => new FuncionarioResponse(
                x.Id,
                x.Nome,
                x.Telefone,
                x.Email,
                x.CPF,
                x.Cargo.ToString()
            ))
            .ToListAsync();

        return Results.Ok(funcionarios);
    }

    // ================= GET BY ID =================

    private static async Task<IResult> GetById(Guid id, AppDbContext db)
    {
        var funcionario = await db.Funcionarios
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new FuncionarioResponse(
                x.Id,
                x.Nome,
                x.Telefone,
                x.Email,
                x.CPF,
                x.Cargo.ToString()
            ))
            .FirstOrDefaultAsync();

        return funcionario is null ? Results.NotFound() : Results.Ok(funcionario);
    }

    // ================= CREATE =================

    private static async Task<IResult> Create(CreateFuncionarioRequest req, AppDbContext db)
    {
        try
        {
            if (!Enum.TryParse<Cargo>(req.Cargo, true, out var cargo))
                return Results.BadRequest("Cargo inválido");

            var funcionario = new Funcionario(
                req.Nome,
                req.Telefone,
                req.Email,
                req.Cpf,
                cargo
            );

            db.Funcionarios.Add(funcionario);
            await db.SaveChangesAsync();

            return Results.Created($"/api/funcionarios/{funcionario.Id}",
                new FuncionarioResponse(
                    funcionario.Id,
                    funcionario.Nome,
                    funcionario.Telefone,
                    funcionario.Email,
                    funcionario.CPF,
                    funcionario.Cargo.ToString()
                ));
        }
        catch (DomainException ex)
        {
            return Results.BadRequest(new { erro = ex.Message });
        }
    }

    // ================= UPDATE =================

    private static async Task<IResult> Update(Guid id, UpdateFuncionarioRequest req, AppDbContext db)
    {
        var funcionario = await db.Funcionarios.FindAsync(id);
        if (funcionario is null)
            return Results.NotFound();

        try
        {
            if (!Enum.TryParse<Cargo>(req.Cargo, true, out var cargo))
                return Results.BadRequest("Cargo inválido");

            funcionario.Atualizar(
                req.Nome,
                req.Telefone,
                req.Email,
                req.Cpf,
                cargo
            );

            await db.SaveChangesAsync();
            return Results.NoContent();
        }
        catch (DomainException ex)
        {
            return Results.BadRequest(new { erro = ex.Message });
        }
    }

    // ================= DELETE =================

    private static async Task<IResult> Delete(Guid id, AppDbContext db)
    {
        var funcionario = await db.Funcionarios.FindAsync(id);
        if (funcionario is null)
            return Results.NotFound();

        db.Funcionarios.Remove(funcionario);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static async Task<IResult> FuncionariosMaisVendem(IMediator mediator)
    {
        var result = await mediator.Send(new FuncionariosMaisVendemQuery());
        return Results.Ok(result);
    }

    // ================= REQUESTS =================

    public record CreateFuncionarioRequest(
        string Nome,
        string Telefone,
        string Email,
        string Cpf,
        string Cargo
    );

    public record UpdateFuncionarioRequest(
        string Nome,
        string Telefone,
        string Email,
        string Cpf,
        string Cargo
    );

    public record FuncionarioResponse(
        Guid Id,
        string Nome,
        string Telefone,
        string Email,
        string CPF,
        string Cargo
    );
}