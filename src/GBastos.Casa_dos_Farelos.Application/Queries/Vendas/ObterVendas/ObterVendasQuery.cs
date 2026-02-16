using GBastos.Casa_dos_Farelos.Shared.Dtos.Vendas;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Vendas.ObterVendas;

public sealed record ObterVendasQuery(
    DateTime? DataInicio,
    DateTime? DataFim
) : IRequest<List<VendaDto>>;