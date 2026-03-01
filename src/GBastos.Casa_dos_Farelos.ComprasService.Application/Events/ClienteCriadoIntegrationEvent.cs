namespace GBastos.Casa_dos_Farelos.ComprasService.Application.Events;

public sealed class ClienteCriadoIntegrationEvent
{
    public Guid ClienteId { get; }
    public string Nome { get; }
    public string Email { get; }

    public ClienteCriadoIntegrationEvent(
        Guid clienteId,
        string nome,
        string email)
    {
        ClienteId = clienteId;
        Nome = nome;
        Email = email;
    }
}