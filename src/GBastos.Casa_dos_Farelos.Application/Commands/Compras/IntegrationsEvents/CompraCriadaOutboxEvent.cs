using GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Compras.IntegrationsEvents;

/// <summary>
/// Evento disparado quando uma compra é criada, usado no Outbox.
/// </summary>
public sealed class CompraCriadaOutboxEvent : IIntegrationEvent
{
    // Identificador único do evento
    public Guid Id { get; } = Guid.NewGuid();

    // Momento em que o evento ocorreu
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;

    // Nome estável do contrato do evento
    public static string Name => nameof(CompraCriadaOutboxEvent);

    // Dados específicos da compra
    public Guid CompraId { get; }
    public Guid FornecedorId { get; }
    public decimal Total { get; }
    public List<ItemCompraDto> Itens { get; }

    // Propriedades do IIntegrationEvent
    public string EventType => EventType;
    public int Version { get; } = 1;

    // Construtor
    public CompraCriadaOutboxEvent(Guid compraId, Guid fornecedorId, decimal total, List<ItemCompraDto> itens)
    {
        CompraId = compraId;
        FornecedorId = fornecedorId;
        Total = total;
        Itens = itens ?? new List<ItemCompraDto>();
    }
}