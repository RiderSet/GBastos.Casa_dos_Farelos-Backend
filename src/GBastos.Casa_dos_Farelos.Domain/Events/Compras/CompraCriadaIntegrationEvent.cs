using GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;
using GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Compras;

public sealed record CompraCriadaIntegrationEvent(
    Guid CompraId,
    Guid FornecedorId,
    decimal ValorTotal,
    List<ItemCompraDto> Itens
) : BaseIntegrationEvent;