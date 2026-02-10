using GBastos.Casa_dos_Farelos.Application.Dtos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Vendas.ObterVendas;

public sealed record ObterVendaPorIdQuery(Guid Id) : IRequest<VendaDetalhadaDto?>;