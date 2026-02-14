using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class ItemVenda : BaseEntity
{
    public Guid VendaId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public string DescricaoProduto { get; private set; } = string.Empty;

    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }

    public decimal SubTotal { get; private set; }

    public Venda Venda { get; private set; } = null!;
    public Produto Produto { get; private set; } = null!;

    protected ItemVenda() { }

    public ItemVenda(Guid produtoId, int quantidade, decimal precoUnitario)
    {
        if (produtoId == Guid.Empty)
            throw new DomainException("Produto inválido.");

        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        if (precoUnitario <= 0)
            throw new DomainException("Preço inválido.");

        ProdutoId = produtoId;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
        SubTotal = quantidade * precoUnitario;
    }

    internal void DefinirVenda(Guid vendaId)
    {
        if (vendaId == Guid.Empty)
            throw new DomainException("Venda inválida.");

        VendaId = vendaId;
    }
}