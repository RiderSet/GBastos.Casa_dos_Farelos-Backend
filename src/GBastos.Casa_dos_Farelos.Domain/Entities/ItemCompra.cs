using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Entities;

public class ItemCompra
{
    public Guid ProdutoId { get; private set; }
    public Produto Produto { get; private set; } = null!;

    public string NomeProduto { get; private set; } = string.Empty;
    public int Quantidade { get; private set; }
    public decimal CustoUnitario { get; private set; }

    // RELACIONAMENTO COM A COMPRA
    public Guid CompraId { get; private set; }
    public Compra Compra { get; private set; } = null!;

    public decimal SubTotal => Quantidade * CustoUnitario;

    protected ItemCompra() { } // EF

    public ItemCompra(Guid produtoId, string nomeProduto, int quantidade, decimal custoUnitario)
    {
        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
        CustoUnitario = custoUnitario;
    }

    // 🔹 Somente o agregado raiz deve chamar
    internal void DefinirCompra(Guid compraId)
    {
        if (compraId == Guid.Empty)
            throw new DomainException("Compra inválida.");

        CompraId = compraId;
    }

    public void SomarQuantidade(int quantidade, decimal novoCusto)
    {
        Quantidade += quantidade;
        CustoUnitario = novoCusto;
    }
}