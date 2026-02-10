using GBastos.Casa_dos_Farelos.Application.Dtos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Vendas.ObterVendas;

public sealed record ObterVendasQuery(
    DateTime? DataInicio,
    DateTime? DataFim
) : IRequest<List<VendaDto>>;