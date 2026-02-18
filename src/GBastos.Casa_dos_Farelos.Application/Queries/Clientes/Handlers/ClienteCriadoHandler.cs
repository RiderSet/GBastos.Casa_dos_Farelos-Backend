using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Shared.Events.Clientes;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Clientes.Handlers;

public sealed class ClienteCriadoHandler : INotificationHandler<ClienteCriadoEvent>
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteCriadoHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task Handle(ClienteCriadoEvent notification, CancellationToken ct)
    {
        Cliente cliente;

        if (notification.Tipo == "PF")
        {
            cliente = ClientePF.CriarClientePF(
                notification.Nome,
                notification.Telefone,
                notification.Email,
                notification.Documento,
                DateTime.UtcNow.AddYears(-18) // exemplo mínimo — ideal vir no evento
            );
        }
        else if (notification.Tipo == "PJ")
        {
            cliente = ClientePJ.CriarClientePJ(
                notification.Nome,          // Razão social
                notification.Telefone,
                notification.Email,
                notification.Nome,          // Nome fantasia (ajuste depois no evento real)
                notification.Documento,
                notification.Nome           // contato
            );
        }
        else
        {
            throw new ArgumentException($"Tipo de cliente inválido: {notification.Tipo}");
        }

        await _clienteRepository.AddAsync(cliente, ct);
    }
}