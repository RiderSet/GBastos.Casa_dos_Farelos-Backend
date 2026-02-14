namespace GBastos.Casa_dos_Farelos.Application.Commands.Compras.IntegrationsEvents;

public sealed record CompraCriadaIntegrationEvent(
    Guid CompraId,
    Guid FornecedorId,
    decimal Total,
    IReadOnlyCollection<CompraItemDto> Itens
);