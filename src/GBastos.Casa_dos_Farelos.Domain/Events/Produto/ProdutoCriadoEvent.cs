using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Produto;

public sealed record ProdutoCriadoEvent(Guid ProdutoId, string ProdutoNome, decimal PrecoVenda) : DomainEvent
{
    public decimal PrecoVenda { get; private set; } = PrecoVenda;
}
