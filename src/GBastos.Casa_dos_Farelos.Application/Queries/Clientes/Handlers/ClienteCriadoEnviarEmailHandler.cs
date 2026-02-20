using GBastos.Casa_dos_Farelos.Application.Abstraction;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Events.Clientes;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Clientes.Handlers;

public sealed class ClienteCriadoEnviarEmailHandler
    : IIntegrationEventHandler<ClienteCriadoEvent>
{
    private readonly IEmailService _emailService;

    public ClienteCriadoEnviarEmailHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task HandleAsync(ClienteCriadoEvent evt, CancellationToken ct)
    {
        await _emailService.EnviarAsync(
            evt.Email,
            evt.Nome,
            ct);
    }
}