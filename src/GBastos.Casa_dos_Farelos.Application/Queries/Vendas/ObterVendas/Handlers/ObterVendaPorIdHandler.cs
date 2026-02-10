using GBastos.Casa_dos_Farelos.Application.Dtos;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Vendas.ObterVendas.Handlers;

public sealed class ObterVendaPorIdHandler
    : IRequestHandler<ObterVendaPorIdQuery, VendaDetalhadaDto?>
{
    private readonly IVendaReadRepository _repo;

    public ObterVendaPorIdHandler(IVendaReadRepository repo)
    {
        _repo = repo;
    }

    public Task<VendaDetalhadaDto?> Handle(
        ObterVendaPorIdQuery request,
        CancellationToken cancellationToken)
        => _repo.ObterPorIdAsync(request.Id, cancellationToken);
}