using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Clientes.CriarCliente.Handlers;

public sealed class CriarClientePFHandler : IRequestHandler<CriarClientePFCommand, Guid>
{
    private readonly IClientePFRepository _repo;
    private readonly IUnitOfWork _uow;

    public CriarClientePFHandler(IClientePFRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Guid> Handle(CriarClientePFCommand request, CancellationToken ct)
    {
        var cliente = ClientePF.Criar(
            request.Nome,
            request.Telefone,
            request.Email,
            request.Cpf
        );

        await _repo.AddAsync(cliente, ct);

        // 🔥 Aqui acontece o commit REAL
        await _uow.SaveChangesAsync(ct);

        return cliente.Id;
    }
}