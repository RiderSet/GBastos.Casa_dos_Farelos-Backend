using GBastos.Casa_dos_Farelos.Application.Commands.Vendas.CriarVenda;
using GBastos.Casa_dos_Farelos.Application.Queries.Vendas.ObterVendas;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Vendas;

public static class VendaEndpoints
{
    public static IEndpointRouteBuilder MapVendaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/vendas")
                       .WithTags("Vendas")
                       .RequireAuthorization();

        // ================= LISTAR =================
        group.MapGet("/", GetAll);

        // ================= DETALHE =================
        group.MapGet("/{id:guid}", GetById);

        // ================= CRIAR =================
        group.MapPost("/", Create)
             .RequireAuthorization("Vendedor");

        return app;
    }

    // =========================================================
    private static async Task<IResult> GetAll(
        DateTime? dataInicio,
        DateTime? dataFim,
        IMediator mediator)
    {
        var vendas = await mediator.Send(new ObterVendasQuery(dataInicio, dataFim));
        return Results.Ok(vendas);
    }

    // =========================================================
    private static async Task<IResult> GetById(
        Guid id,
        IMediator mediator)
    {
        var venda = await mediator.Send(new ObterVendaPorIdQuery(id));
        return venda is null
            ? Results.NotFound()
            : Results.Ok(venda);
    }

    // =========================================================
    private static async Task<IResult> Create(
        [FromBody] CriarVendaCommand command,
        IMediator mediator)
    {
        var id = await mediator.Send(command);
        return Results.Created($"/api/vendas/{id}", new { id });
    }
}