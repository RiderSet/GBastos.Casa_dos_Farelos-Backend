using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class ItemPedido : BaseEntity
{
    public Guid PedidoId { get; private set; }
    public Guid ProdutoId { get; private set; }

    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }
    public decimal SubTotal { get; private set; }

    public Pedido Pedido { get; private set; } = null!;
    public Produto Produto { get; private set; } = null!;

    protected ItemPedido() { }

    public ItemPedido(Guid pedidoId, Guid produtoId, int quantidade, decimal preco)
    {
        PedidoId = pedidoId;
        ProdutoId = produtoId;
        Quantidade = quantidade;
        PrecoUnitario = preco;
        SubTotal = quantidade * preco;
    }
}
