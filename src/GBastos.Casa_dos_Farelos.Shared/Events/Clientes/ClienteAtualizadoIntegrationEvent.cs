using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Shared.Events.Clientes;

/// <summary>
/// Evento disparado quando um cliente existente é atualizado.
/// </summary>
public sealed class ClienteAtualizadoIntegrationEvent : IIntegrationEvent
{
    // Dados do cliente atualizado
    public Guid ClienteId { get; }
    public string Nome { get; }
    public string CPF { get; }
    public DateTime AtualizadoEm { get; }

    // Propriedades do evento
    public Guid Id { get; }
    public DateTime OccurredOnUtc { get; }
    public string EventType => GetType().Name; 
    public int Version { get; } = 1;

    public ClienteAtualizadoIntegrationEvent(Guid clienteId, string nome, string cpf, DateTime atualizadoEm)
    {
        Id = Guid.NewGuid();              
        OccurredOnUtc = DateTime.UtcNow;  
        ClienteId = clienteId;
        Nome = nome;
        CPF = cpf;
        AtualizadoEm = atualizadoEm;
    }
}