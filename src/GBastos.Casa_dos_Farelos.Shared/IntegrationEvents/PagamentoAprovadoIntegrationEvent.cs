namespace GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

public sealed record PagamentoAprovadoIntegrationEvent(
    Guid Id,
    Guid PedidoId,
    Guid ClienteId,
    decimal ValorPg,
    DateTime CriadoEmUtc
);