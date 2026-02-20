using GBastos.Casa_dos_Farelos.Domain.Interfaces;

namespace GBastos.Casa_dos_Farelos.Domain.Events;

public sealed class EstoqueBaixadoDomainEvent : IDomainEvent
{
    public Guid ProdutoId { get; }
    public string NomeProduto { get; }
    public int Quantidade { get; }
    public DateTime OccurredOnUtc { get; }

    public EstoqueBaixadoDomainEvent(Guid produtoId, string nomeProduto, int quantidade)
    {
        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
        OccurredOnUtc = DateTime.UtcNow;
    }
}