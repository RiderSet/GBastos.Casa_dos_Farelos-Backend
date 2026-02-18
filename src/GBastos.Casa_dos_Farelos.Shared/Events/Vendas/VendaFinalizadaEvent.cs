using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Shared.Events.Vendas;

public sealed record VendaFinalizadaEvent(
    Guid VendaId,
    Guid ClienteId,
    decimal Total,
    DateTime DataVenda
) : IIntegrationEvent
{
    public static string Name => "venda.finalizada.v1";
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

    public Guid Id => throw new NotImplementedException();
    public string EventType => throw new NotImplementedException();
    public int Version => throw new NotImplementedException();

    public DateTime OccurredOnUtc => throw new NotImplementedException();
}