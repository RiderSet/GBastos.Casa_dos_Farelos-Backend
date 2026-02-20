using GBastos.Casa_dos_Farelos.Application.Commands.Compras.CriarCompra;
using GBastos.Casa_dos_Farelos.Application.Queries.Compras.ObterCompras;
using GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Compras;

public static class CompraEndpoints
{
    public static IEndpointRouteBuilder MapCompraEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/compras")
                       .WithTags("Compras")
                       .RequireAuthorization();

        group.MapGet("", async (
            [FromQuery] DateTime? dataInicio,
            [FromQuery] DateTime? dataFim,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var compras = await mediator.Send(new ObterComprasQuery(dataInicio, dataFim), ct);
            return Results.Ok(compras);
        });

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var compra = await mediator.Send(new ObterCompraPorIdQuery(id), ct);
            return compra is null ? Results.NotFound() : Results.Ok(compra);
        })
        .WithName("ObterCompraPorId")
        .Produces<CompraDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("", async (
            CriarCompraCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.CreatedAtRoute("ObterCompraPorId", new { id }, new { id });
        })
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        return app;
    }
}