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
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.ClientePJ);

        var dto = request.ClientePJ;

        var cliente = ClientePJ.CriarClientePJ(
            dto.RazaoSocial,
            dto.NomeFantasia,
            dto.Telefone,
            dto.Email,
            dto.CNPJ,
            dto.Contato
        );

        await _repo.AddAsync(cliente, ct).ConfigureAwait(false);

        return cliente.Id;
    }
}