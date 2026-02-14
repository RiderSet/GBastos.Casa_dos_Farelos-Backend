using GBastos.Casa_dos_Farelos.Application.Dtos;
using GBastos.Casa_dos_Farelos.Application.Interfaces;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Compras.IntegrationsEvents;

public sealed class CompraCriadaOutboxEvent : IIntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public Guid CompraId { get; }
    public Guid FornecedorId { get; }
    public decimal Total { get; }
    public List<CompraItemDto> Itens { get; }

    public CompraCriadaOutboxEvent(Guid compraId, Guid fornecedorId, decimal total, List<CompraItemDto> itens)
    {
        CompraId = compraId;
        FornecedorId = fornecedorId;
        Total = total;
        Itens = itens;
    }
}