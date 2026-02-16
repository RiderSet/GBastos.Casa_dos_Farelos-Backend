using GBastos.Casa_dos_Farelos.Domain.Dtos;
using GBastos.Casa_dos_Farelos.Shared.Dtos;
using GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Compras.IntegrationsEvents;

public sealed class CompraCriadaOutboxEvent : IIntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    // Nome estável do contrato do evento
    public static string Name => throw new NotImplementedException();

    public Guid CompraId { get; }
    public Guid FornecedorId { get; }
    public decimal Total { get; }
    public List<CompraItemDto> Itens { get; }

    public string EventType => throw new NotImplementedException();

    public int Version => throw new NotImplementedException();

    public CompraCriadaOutboxEvent(Guid compraId, Guid fornecedorId, decimal total, List<CompraItemDto> itens)
    {
        CompraId = compraId;
        FornecedorId = fornecedorId;
        Total = total;
        Itens = itens;
    }
}