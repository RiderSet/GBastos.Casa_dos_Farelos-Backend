using GBastos.Casa_dos_Farelos.Domain.Dtos;
using GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Compras.IntegrationsEvents;

/// <summary>
/// Evento disparado quando uma nova compra é criada.
/// </summary>
public sealed class CompraCriadaIntegrationEvent : IIntegrationEvent
{
    // Identificador único do evento
    public Guid Id { get; }

    // Momento em que o evento ocorreu
    public DateTime OccurredOnUtc { get; }

    // Nome estável do contrato do evento
    public static string Name => nameof(CompraCriadaIntegrationEvent);

    // Dados da compra
    public Guid CompraId { get; }
    public Guid FornecedorId { get; }
    public decimal Total { get; }
    public List<ItemCompraDto> Itens { get; }

    // Propriedades do IIntegrationEvent
    public string EventType => Name;
    public int Version { get; } = 1;

    // Construtor
    public CompraCriadaIntegrationEvent(Guid compraId, Guid fornecedorId, decimal total, List<CompraItemDto> itens)
    {
        Id = Guid.NewGuid();
        OccurredOnUtc = DateTime.UtcNow;

        CompraId = compraId;
        FornecedorId = fornecedorId;
        Total = total;
        Itens = itens ?? new List<ItemCompraDto>();
    }
}