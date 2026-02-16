using GBastos.Casa_dos_Farelos.Domain.Abstractions;

namespace GBastos.Casa_dos_Farelos.Shared.Events.Clientes;

public sealed record ClienteCriadoEvent(
    Guid ClienteId,
    string Name,
    string Documento,
    string Tipo
) : BaseIntegrationEvent;