namespace GBastos.Casa_dos_Farelos.Domain.Events.Compras;

public class CompraItemSnapshot
{
    public Guid ProdutoId { get; }
    public string NomeProduto { get; }
    public int Quantidade { get; }
    public decimal CustoUnitario { get; }
    public decimal SubTotal { get; }

    public CompraItemSnapshot(Guid produtoId, string nomeProduto, int quantidade, decimal custoUnitario, decimal subTotal)
    {
        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
        CustoUnitario = custoUnitario;
        SubTotal = subTotal;
    }
}