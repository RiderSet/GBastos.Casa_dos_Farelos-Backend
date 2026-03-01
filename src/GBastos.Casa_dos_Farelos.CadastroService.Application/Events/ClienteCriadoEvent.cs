using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.CadastroService.Application.Events;

public sealed class ClienteCriadoEvent : IDomainEvent
{
    public Guid ClienteId { get; }

    public Guid EventId => throw new NotImplementedException();

    public DateTime OccurredOnUtc => DateTime.UtcNow;

    public ClienteCriadoEvent(Guid clienteId)
    {
        ClienteId = clienteId;
    }
}