using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.ComprasService.Domain.Events;

public record EstoqueBaixadoDomainEvent(
    Guid ProdutoId,
    string NomeProduto,
    int QuantidadeBaixada
) : IDomainEvent{}
