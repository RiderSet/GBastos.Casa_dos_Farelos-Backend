using GBastos.Casa_dos_Farelos.Application.Queries.Vendas.ObterVendas;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Vendas;

public static class VendaQueryEndpoints
{
    public static IEndpointRouteBuilder MapVendaQueryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/vendas/relatorios")
                       .WithTags("Relatórios de Vendas")
                       .RequireAuthorization();

        group.MapGet("/", Obter);

        return app;
    }

    private static async Task<IResult> Obter(
        DateTime? inicio,
        DateTime? fim,
        IMediator mediator)
    {
        var vendas = await mediator.Send(new ObterVendasQuery(inicio, fim));
        return Results.Ok(vendas);
    }
}
