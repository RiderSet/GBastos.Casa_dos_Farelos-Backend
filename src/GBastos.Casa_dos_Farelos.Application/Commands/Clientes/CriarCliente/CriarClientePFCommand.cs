using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Clientes.CriarCliente;

public record CriarClientePFCommand(
    string Nome,
    string Telefone,
    string Email,
    string Cpf,
        DateTime dtNascimento
) : IRequest<Guid>;