using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Produto;

public sealed class ProdutoCriadoEvent : DomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

    public Guid ProdutoId { get; init; }
    public string ProdutoNome { get; init; }
    public decimal PrecoVenda { get; private set; }

    public ProdutoCriadoEvent(string produtonome, decimal precovenda)
    {
        ProdutoNome = produtonome;
        PrecoVenda = precovenda;
    }
}
