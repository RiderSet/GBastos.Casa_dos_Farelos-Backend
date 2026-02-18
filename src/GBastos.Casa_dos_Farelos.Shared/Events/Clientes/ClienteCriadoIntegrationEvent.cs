using GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.Shared.Events.Clientes;

public sealed class ClienteCriadoIntegrationEvent : IntegrationEvent
{
    public Guid ClienteId { get; }
    public string Nome { get; }
    public string CPF { get; }
    public DateTime CriadoEm { get; }

    public ClienteCriadoIntegrationEvent(Guid clienteId, string nome, string cpf, DateTime criadoEm)
    {
        ClienteId = clienteId;
        Nome = nome;
        CPF = cpf;
        CriadoEm = criadoEm;
    }
}