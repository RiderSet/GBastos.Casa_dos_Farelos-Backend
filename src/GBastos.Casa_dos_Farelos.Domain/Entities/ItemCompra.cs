using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class ItemCompra : BaseEntity
{
    public Guid CompraId { get; private set; }
    public Guid ProdutoId { get; private set; }
    public string NomeProduto { get; private set; } = null!;
    public int Quantidade { get; private set; }
    public decimal CustoUnitario { get; private set; }
    public decimal SubTotal => Quantidade * CustoUnitario;

    public Produto Produto { get; private set; } = null!;
    public Compra Compra { get; private set; } = null!;

    protected ItemCompra() { }

    public ItemCompra(Guid produtoId, string nomeProduto, int quantidade, decimal custoUnitario)
    {
        if (produtoId == Guid.Empty)
            throw new DomainException("Produto inválido.");

        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        if (custoUnitario <= 0)
            throw new DomainException("Custo inválido.");

        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
        CustoUnitario = custoUnitario;
    }

    public void DefinirCompra(Guid compraId)
    {
        CompraId = compraId;
    }
}