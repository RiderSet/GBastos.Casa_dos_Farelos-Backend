using GBastos.Casa_dos_Farelos.Application.Commands.Clientes.CriarCliente;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using MediatR;

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

        if (existe is not null)
            throw new DomainException("Já existe um cliente cadastrado com este CPF.");

        var cliente = new ClientePF(
            request.Nome,
            request.Telefone,
            request.Email,
            request.CPF,
            request.DtNascimento
        );

        await _repo.AddAsync(cliente, ct);

        return cliente.Id;
    }
}