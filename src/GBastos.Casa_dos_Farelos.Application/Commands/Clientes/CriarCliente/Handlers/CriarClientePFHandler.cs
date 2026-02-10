using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Clientes.CriarCliente.Handlers;

public sealed class CriarClientePFHandler : IRequestHandler<CriarClientePFCommand, Guid>
{
    private readonly IClientePFRepository _repo;

    public CriarClientePFHandler(IClientePFRepository repo)
    {
        _repo = repo;
    }

    public async Task<Guid> Handle(CriarClientePFCommand request, CancellationToken ct)
    {
        var existe = await _repo.ObterPorCpfAsync(request.CPF, ct);
        if (existe != null)
            throw new Exception("CPF já cadastrado.");

        var cliente = new ClientePF(
            request.Nome,
            request.CPF,
            request.Telefone,
            request.Email,
            request.DataNascimento
        );

        await _repo.AddAsync(cliente, ct);

        return cliente.Id;
    }
}
