using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Dtos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Clientes.Handlers;

public sealed class ListarClientesHandler : IRequestHandler<ListarClientesQuery, List<ClienteListDto>>
{
    private readonly IClientePFRepository _repoPF;
    private readonly IClientePJRepository _repoPJ;

    public ListarClientesHandler(IClientePFRepository repoPF, IClientePJRepository repoPJ)
    {
        _repoPF = repoPF;
        _repoPJ = repoPJ;
    }

    public async Task<List<ClienteListDto>> Handle(ListarClientesQuery request, CancellationToken ct)
    {
        var clientesPF = await _repoPF.ListarAsync(ct);
        var clientesPJ = await _repoPJ.ListarAsync(ct);

        var resultado = new List<ClienteListDto>();

        resultado.AddRange(clientesPF.Select(c => new ClienteListDto(
            Id: c.Id,
            Nome: c.Nome,
            Telefone: c.Telefone,
            Email: c.Email,
            Tipo: "PF",
            Documento: c.CPF
        )));

        resultado.AddRange(clientesPJ.Select(c => new ClienteListDto(
            Id: c.Id,
            Nome: c.NomeFantasia,
            Telefone: c.Telefone,
            Email: c.Email,
            Tipo: "PJ",
            Documento: c.CNPJ,
            Contato: c.Contato
        )));

        return resultado;
    }
}