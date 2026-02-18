using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Shared.Events.Vendas;

/// <summary>
/// Evento disparado quando uma nova venda é criada.
/// </summary>
public sealed class VendaCriadaIntegrationEvent : IIntegrationEvent
{
    public Guid VendaId { get; }
    public Guid ClienteId { get; }
    public decimal ValorTotal { get; }
    public DateTime CriadoEm { get; }

    // Propriedades do evento
    public Guid Id { get; }
    public DateTime OccurredOnUtc { get; }
    public string EventType => GetType().Name;
    public int Version { get; } = 1;

    public VendaCriadaIntegrationEvent(Guid vendaId, Guid clienteId, decimal valorTotal, DateTime criadoEm)
    {
        VendaId = vendaId;
        ClienteId = clienteId;
        ValorTotal = valorTotal;
        CriadoEm = criadoEm;
    }
}