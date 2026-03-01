using GBastos.Casa_dos_Farelos.SharedKernel.Messaging;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events.Pedidos;

public sealed record PedidoCriadoIntegrationEvent : IntegrationEvent
{
    public Guid PedidoId { get; init; }
    public decimal Total { get; init; }
}