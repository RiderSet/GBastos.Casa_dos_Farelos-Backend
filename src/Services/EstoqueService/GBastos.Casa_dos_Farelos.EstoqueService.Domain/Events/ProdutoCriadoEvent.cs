using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed class ProdutoCriadoEvent : IDomainEvent
{
    public Guid ProdutoId { get; }
    public string Nome { get; }
    public decimal PrecoVenda { get; }
    public DateTime OccurredOnUtc { get; }

    public Guid EventId => Guid.NewGuid();

    public ProdutoCriadoEvent(Guid produtoId, string nome, decimal precoVenda)
    {
        ProdutoId = produtoId;
        Nome = nome;
        PrecoVenda = precoVenda;
        OccurredOnUtc = DateTime.UtcNow;
    }
}