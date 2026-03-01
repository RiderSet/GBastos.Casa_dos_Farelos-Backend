using GBastos.Casa_dos_Farelos.SharedKernel.Messaging;

namespace GBastos.Casa_dos_Farelos.SharedKernel.DomainEvents;

public sealed record PagamentoAprovadoIntegrationEvent(
    Guid PagamentoId,
    Guid PedidoId,
    decimal Valor
) : IntegrationEvent;