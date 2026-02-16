using GBastos.Casa_dos_Farelos.Domain.Abstractions;
using GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;

namespace GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

public sealed record CompraCriadaIntegrationEvent(
    Guid CompraId,
    Guid FornecedorId,
    decimal ValorTotal,
    List<CompraItemDto> Itens
) : BaseIntegrationEvent;