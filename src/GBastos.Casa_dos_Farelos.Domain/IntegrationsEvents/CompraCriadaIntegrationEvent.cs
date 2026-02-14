using GBastos.Casa_dos_Farelos.Application.Interfaces;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Compras.IntegrationsEvents;

public sealed record CompraCriadaIntegrationEvent(
    Guid CompraId,
    Guid FornecedorId,
    decimal ValorTotal,
    List<CompraItemDto> Itens
) : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}