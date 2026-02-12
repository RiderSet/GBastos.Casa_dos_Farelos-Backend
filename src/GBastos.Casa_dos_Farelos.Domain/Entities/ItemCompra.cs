using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class ItemCompra : Entity
{
    public Guid CompraId { get; private set; }
    public Guid ProdutoId { get; private set; }

    public int Quantidade { get; private set; }
    public decimal CustoUnitario { get; private set; }

    // calculado SEMPRE
    public decimal SubTotal => Quantidade * CustoUnitario;

    public Compra Compra { get; private set; } = null!;
    public Produto Produto { get; private set; } = null!;

    protected ItemCompra() { }

    internal ItemCompra(Guid produtoId, int quantidade, decimal custoUnitario)
    {
        if (produtoId == Guid.Empty)
            throw new DomainException("Produto inválido.");

        if (quantidade <= 0)
            throw new DomainException("Quantidade deve ser maior que zero.");

        if (custoUnitario <= 0)
            throw new DomainException("Custo unitário inválido.");

        ProdutoId = produtoId;
        Quantidade = quantidade;
        CustoUnitario = custoUnitario;
    }

    internal void DefinirCompra(Guid compraId)
    {
        if (compraId == Guid.Empty)
            throw new DomainException("Compra inválida.");

        CompraId = compraId;
    }

    internal void SomarQuantidade(int quantidade, decimal custoUnitario)
    {
        if (quantidade <= 0)
            throw new DomainException("Quantidade deve ser maior que zero.");

        if (custoUnitario <= 0)
            throw new DomainException("Custo unitário deve ser maior que zero.");

        // regra: mesma compra + mesmo produto = acumula quantidade
        Quantidade += quantidade;

        // regra de negócio comum em compras:
        // o último custo pago vira o custo atual do lote
        CustoUnitario = custoUnitario;
    }
}