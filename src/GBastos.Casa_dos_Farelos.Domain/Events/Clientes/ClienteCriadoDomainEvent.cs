using GBastos.Casa_dos_Farelos.Domain.Abstractions;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Clientes;

public sealed class ClienteCriadoDomainEvent : IDomainEvent
{
    public Guid ClienteId { get; }
    public string Nome { get; }
    public string Documento { get; }
    public string Tipo { get; }
    public DateTime OccurredOnUtc { get; }

    public ClienteCriadoDomainEvent(
        Guid clienteId,
        string nome,
        string documento,
        string tipo)
    {
        ClienteId = clienteId;
        Nome = nome;
        Documento = documento;
        Tipo = tipo;
        OccurredOnUtc = DateTime.UtcNow;
    }
}