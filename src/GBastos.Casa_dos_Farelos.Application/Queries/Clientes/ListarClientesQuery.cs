using GBastos.Casa_dos_Farelos.Application.Dtos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Clientes;

public sealed record ListarClientesQuery() : IRequest<List<ClientePFDto>>;