using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Shared.Events.Clientes;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Clientes.Handlers;

public sealed class ClienteCriadoHandler
    : IIntegrationEventHandler<ClienteCriadoEvent>
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteCriadoHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task HandleAsync(ClienteCriadoEvent notification, CancellationToken ct)
    {
        Cliente cliente;

        if (notification.Tipo == "PF")
        {
            cliente = ClientePF.CriarClientePF(
                notification.Nome,
                notification.Telefone,
                notification.Email,
                notification.Documento,
                DateTime.UtcNow.AddYears(-18)
            );
        }
        else if (notification.Tipo == "PJ")
        {
            cliente = ClientePJ.CriarClientePJ(
                notification.Nome,
                notification.Telefone,
                notification.Email,
                notification.Nome,
                notification.Documento,
                notification.Nome
            );
        }
        else
        {
            throw new ArgumentException($"Tipo de cliente inválido: {notification.Tipo}");
        }

        await _clienteRepository.AddAsync(cliente, ct);
    }
}