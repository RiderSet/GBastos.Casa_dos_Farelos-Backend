using GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Api.Endpoints;

public static class EstoqueEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/estoque/reservar",
        async (ReservarEstoqueCommand cmd, IMediator mediator) =>
        {
            return await mediator.Send(cmd);
        });
    }
}