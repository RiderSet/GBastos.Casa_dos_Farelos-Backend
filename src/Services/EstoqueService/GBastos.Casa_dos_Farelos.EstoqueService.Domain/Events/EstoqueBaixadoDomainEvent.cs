using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed class EstoqueBaixadoDomainEvent : IDomainEvent
{
    public Guid ProdutoId { get; }
    public string NomeProduto { get; }
    public int QuantidadeBaixada { get; }
    public DateTime OccurredOn { get; }

    public EstoqueBaixadoDomainEvent(
        Guid produtoId,
        string nomeProduto,
        int quantidadeBaixada)
    {
        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        QuantidadeBaixada = quantidadeBaixada;
        OccurredOn = DateTime.UtcNow;
    }
}