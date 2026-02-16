using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Events.Clientes;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Clientes.Handlers;

public sealed class ClienteCriadoEnviarEmailHandler
    : IIntegrationEventHandler<ClienteCriadoEvent>
{
    public Task HandleAsync(ClienteCriadoEvent evt, CancellationToken ct)
    {
        Console.WriteLine($"Enviar email para cliente {evt.Name}");
        return Task.CompletedTask;
    }
}