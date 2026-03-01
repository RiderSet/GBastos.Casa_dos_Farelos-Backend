using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.IntegrationEvents;

public sealed class ProdutoCriadoIntegrationEvent : IntegrationEvent
{
    public Guid ProdutoId { get; init; }
    public string Nome { get; init; } = default!;
    public decimal PrecoVenda { get; init; }
}