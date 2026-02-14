using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Clientes.CriarCliente.Handlers;

public sealed class CriarClientePJHandler : IRequestHandler<CriarClientePJCommand, Guid>
{
    private readonly IClienteRepository _repo;

    public CriarClientePJHandler(IClienteRepository repo)
    {
        _repo = repo;
    }

    public async Task<Guid> Handle(CriarClientePJCommand request, CancellationToken ct)
    {
        var dto = request.ClientePJ;
        var cliente = new ClientePJ(dto.Nome, dto.Telefone, dto.Email, dto.CNPJ, dto.Contato);

        await _repo.AdicionarClientePJAsync(cliente, ct);

        return cliente.Id;
    }
}