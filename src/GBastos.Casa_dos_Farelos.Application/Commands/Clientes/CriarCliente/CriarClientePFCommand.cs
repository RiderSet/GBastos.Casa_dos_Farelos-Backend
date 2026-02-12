using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Clientes.CriarCliente;

public sealed record CriarClientePFCommand(
    string Nome,
    string CPF,
    string Telefone,
    string Email,
    DateTime DtNascimento
) : IRequest<Guid>;