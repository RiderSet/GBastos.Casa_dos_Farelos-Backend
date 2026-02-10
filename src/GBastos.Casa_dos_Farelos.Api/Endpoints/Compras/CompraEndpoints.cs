using GBastos.Casa_dos_Farelos.Application.Commands.Compras.CriarCompra;
using GBastos.Casa_dos_Farelos.Application.Queries.Compras.ObterCompras;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Compras
{
    public static class CompraEndpoints
    {
        public static IEndpointRouteBuilder MapCompraEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/compras")
                           .WithTags("Compras")
                           .RequireAuthorization();

            // =========================
            // LISTAR (por período)
            // =========================
            group.MapGet("/", async (
                [FromQuery] DateTime? dataInicio,
                [FromQuery] DateTime? dataFim,
                IMediator mediator) =>
            {
                var compras = await mediator.Send(new ObterComprasQuery(dataInicio, dataFim));
                return Results.Ok(compras);
            });

            // =========================
            // OBTER DETALHADA
            // =========================
            group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var compra = await mediator.Send(new ObterCompraPorIdQuery(id));
                return compra is null ? Results.NotFound() : Results.Ok(compra);
            });

            // =========================
            // CRIAR COMPRA (ENTRADA DE ESTOQUE)
            // =========================
            group.MapPost("/", async (CriarCompraCommand command, IMediator mediator) =>
            {
                var id = await mediator.Send(command);
                return Results.Created($"/api/compras/{id}", id);
            });

            return app;
        }
    }
}
