using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Pedido : BaseEntity
{
    private readonly List<ItemPedido> _itens = new();

    public Guid ClienteId { get; private set; }
    public DateTime Data { get; private set; }
    public decimal Total { get; private set; }

    public ClientePF? ClientePF { get; private set; }
    public ClientePJ? ClientePJ { get; private set; }

    public IReadOnlyCollection<ItemPedido> Itens => _itens;

    protected Pedido() { } // EF

    public Pedido(Guid clienteId)
    {
        ClienteId = clienteId;
        Data = DateTime.UtcNow;
    }

    public void AdicionarItem(Guid produtoId, int quantidade, decimal preco)
    {
        var item = new ItemPedido(Id, produtoId, quantidade, preco);
        _itens.Add(item);
        RecalcularTotal();
    }

    private void RecalcularTotal()
    {
        Total = _itens.Sum(x => x.SubTotal);
    }
}