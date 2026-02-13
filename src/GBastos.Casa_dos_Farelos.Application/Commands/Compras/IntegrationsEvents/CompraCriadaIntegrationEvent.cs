using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Events.Compras;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Compras.IntegrationsEvents;

public sealed record CompraCriadaIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

    public Guid CompraId { get; init; }
    public Guid FornecedorId { get; init; }
    public decimal ValorTotal { get; init; }
    public List<CompraItemDto> Itens { get; init; }

    public CompraCriadaIntegrationEvent(
        Guid compraId,
        Guid fornecedorId,
        decimal valorTotal,
        List<CompraItemDto> itens)
    {
        CompraId = compraId;
        FornecedorId = fornecedorId;
        ValorTotal = valorTotal;
        Itens = itens;
    }
}