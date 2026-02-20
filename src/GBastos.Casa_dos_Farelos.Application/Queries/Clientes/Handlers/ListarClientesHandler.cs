using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Dtos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Clientes.Handlers;

public sealed class ListarClientesHandler : IRequestHandler<ListarClientesQuery, List<ClienteListDto>>
{
    private readonly IClienteRepository _repo;

    public ListarClientesHandler(IClienteRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<ClienteListDto>> Handle(ListarClientesQuery request, CancellationToken ct)
    {
        var clientes = await _repo.ListarAsync(ct);
        return clientes.ToList();
    }
}