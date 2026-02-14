using GBastos.Casa_dos_Farelos.Application.Common;
using GBastos.Casa_dos_Farelos.Application.Dtos;

namespace GBastos.Casa_dos_Farelos.Application.IntegrationEvents;

public sealed record CompraCriadaIntegrationEvent(
    Guid CompraId,
    Guid FornecedorId,
    decimal ValorTotal,
    List<CompraItemDto> Itens
) : BaseIntegrationEvent;