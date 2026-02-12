using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Events.Compras;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Compra : Entity
{
    public Guid FornecedorId { get; private set; }
    public DateTime DataCompra { get; private set; }

    public Fornecedor Fornecedor { get; private set; } = null!;

    private readonly List<ItemCompra> _itens = new();
    public IReadOnlyCollection<ItemCompra> Itens => _itens.AsReadOnly();

    public decimal TotalCompra => _itens.Sum(i => i.SubTotal);

    protected Compra() { }

    public static Compra Criar(Guid fornecedorId)
    => new Compra(fornecedorId);

    private Compra(Guid fornecedorId)
    {
        if (fornecedorId == Guid.Empty)
            throw new DomainException("Fornecedor inválido.");

        FornecedorId = fornecedorId;
        DataCompra = DateTime.UtcNow;

        AddDomainEvent(new CompraCriadaDomainEvent(Id, fornecedorId));
    }

    public void AdicionarItem(Guid produtoId, int quantidade, decimal custoUnitario)
    {
        if (produtoId == Guid.Empty)
            throw new DomainException("Produto inválido.");

        if (quantidade <= 0)
            throw new DomainException("Quantidade deve ser maior que zero.");

        if (custoUnitario <= 0)
            throw new DomainException("Custo unitário deve ser maior que zero.");

        // evita itens duplicados no mesmo agregado
        var itemExistente = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);

        if (itemExistente is not null)
        {
            itemExistente.SomarQuantidade(quantidade, custoUnitario);
            return;
        }

        var item = new ItemCompra(produtoId, quantidade, custoUnitario);

        // associa ao agregado raiz
        item.DefinirCompra(Id);

        _itens.Add(item);
    }

}