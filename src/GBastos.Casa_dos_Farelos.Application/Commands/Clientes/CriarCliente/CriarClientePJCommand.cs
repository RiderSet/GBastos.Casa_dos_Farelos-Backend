using GBastos.Casa_dos_Farelos.Domain.Dtos;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Clientes.CriarCliente;

public record CriarClientePJCommand(ClientePJCreateDto ClientePJ) : IRequest<Guid>;