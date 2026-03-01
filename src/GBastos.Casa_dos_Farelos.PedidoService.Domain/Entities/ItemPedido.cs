using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Entities;

public class ItemPedido : Entity<Guid>
{
    public Guid PedidoId { get; private set; }
    public Guid ProdutoId { get; private set; }

    public string NomeProduto { get; private set; } = string.Empty;

    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }

    public decimal SubTotal => Quantidade * PrecoUnitario;

    protected ItemPedido() : base(Guid.Empty) { }

    public ItemPedido(
        Guid pedidoId,
        Guid produtoId,
        string nomeProduto,
        int quantidade,
        decimal precoUnitario)
        : base(Guid.NewGuid())
    {
        if (pedidoId == Guid.Empty)
            throw new ArgumentException("Pedido inválido.");

        if (produtoId == Guid.Empty)
            throw new ArgumentException("Produto inválido.");

        if (string.IsNullOrWhiteSpace(nomeProduto))
            throw new ArgumentException("Nome do produto obrigatório.");

        if (quantidade <= 0)
            throw new ArgumentException("Quantidade inválida.");

        if (precoUnitario <= 0)
            throw new ArgumentException("Preço inválido.");

        PedidoId = pedidoId;
        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
    }

    public ItemPedido(Guid id, int quantidade, decimal precoUnitario) : base(id)
    {
    }
}