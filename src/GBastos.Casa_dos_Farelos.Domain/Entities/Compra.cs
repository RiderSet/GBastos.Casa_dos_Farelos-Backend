using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Events.Compras;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Compra : AggregateRoot
{
    public Guid FornecedorId { get; private set; }
    public DateTime DataCompra { get; private set; }
    public bool Finalizada { get; private set; }

    public Fornecedor Fornecedor { get; private set; } = null!;

    private readonly List<ItemCompra> _itens = new();
    public IReadOnlyCollection<ItemCompra> Itens => _itens.AsReadOnly();

    public decimal TotalCompra => _itens.Sum(i => i.SubTotal);

    protected Compra() { } // EF

    private Compra(Guid fornecedorId)
    {
        Id = Guid.NewGuid();
        FornecedorId = fornecedorId;
        DataCompra = DateTime.UtcNow;
        Finalizada = false;
    }

    public static Compra Criar(Guid fornecedorId)
        => new Compra(fornecedorId);

    public void AdicionarItem(Guid produtoId, string nomeProduto, int quantidade, decimal custoUnitario)
    {
        if (Finalizada)
            throw new DomainException("Compra já finalizada.");

        if (produtoId == Guid.Empty)
            throw new DomainException("Produto inválido.");

        if (quantidade <= 0)
            throw new DomainException("Quantidade deve ser maior que zero.");

        if (custoUnitario <= 0)
            throw new DomainException("Custo unitário deve ser maior que zero.");

        var itemExistente = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);

        if (itemExistente is not null)
        {
            itemExistente.SomarQuantidade(quantidade, custoUnitario);
            return;
        }

        var item = new ItemCompra(produtoId, nomeProduto, quantidade, custoUnitario);
        item.DefinirCompra(Id);

        _itens.Add(item);
    }

    public void Finalizar()
    {
        if (Finalizada)
            throw new DomainException("Compra já finalizada.");

        if (!_itens.Any())
            throw new DomainException("Não é possível finalizar uma compra sem itens.");

        Finalizada = true;

        // Dispara o DomainEvent com snapshots e total
        AddDomainEvent(new CompraCriadaDomainEvent(
            Id,
            FornecedorId,
            TotalCompra,
            _itens.Select(i => new CompraItemSnapshot(
                i.ProdutoId,
                i.NomeProduto,
                i.Quantidade,
                i.CustoUnitario,
                i.SubTotal
            )).ToList()
        ));
    }
}