using GBastos.Casa_dos_Farelos.Domain.Interfaces;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Clientes;

public sealed record ClienteCriadoDomainEvent(
    Guid ClienteId,
    string Nome,
    string Documento,
    string Tipo,
    string Email,
    string Telefone
) : DomainEvent;