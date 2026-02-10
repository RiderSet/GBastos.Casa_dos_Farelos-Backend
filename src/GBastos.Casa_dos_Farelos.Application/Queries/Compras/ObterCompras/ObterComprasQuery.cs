using GBastos.Casa_dos_Farelos.Application.Dtos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Compras.ObterCompras;

public sealed record ObterComprasQuery(DateTime? DataInicio, DateTime? DataFim)
    : IRequest<List<CompraResumoDto>>;