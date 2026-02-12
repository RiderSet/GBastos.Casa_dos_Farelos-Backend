using GBastos.Casa_dos_Farelos.Application.Dtos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Compras.ObterCompras;

public sealed record ObterCompraPorIdQuery(Guid Id)
    : IRequest<CompraDto?>;