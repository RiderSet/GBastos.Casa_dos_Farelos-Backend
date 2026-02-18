using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Shared.Events.Vendas;

public sealed class VendaAtualizadaIntegrationEvent : IIntegrationEvent
{
    public Guid VendaId { get; }
    public Guid ClienteId { get; }
    public decimal ValorTotal { get; }
    public DateTime AtualizadoEm { get; }

    // Propriedades do evento
    public Guid Id { get; }
    public DateTime OccurredOnUtc { get; }
    public string EventType => GetType().Name;
    public int Version { get; } = 1;

    public VendaAtualizadaIntegrationEvent(Guid vendaId, Guid clienteId, decimal valorTotal, DateTime atualizadoEm)
    {
        VendaId = vendaId;
        ClienteId = clienteId;
        ValorTotal = valorTotal;
        AtualizadoEm = atualizadoEm;
    }
}