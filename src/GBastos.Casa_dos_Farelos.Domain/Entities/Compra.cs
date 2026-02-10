using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Compra : Entity
{
    public Guid FornecedorId { get; private set; }
    public DateTime DataCompra { get; private set; }

    private readonly List<ItemCompra> _itens = new();
    public IReadOnlyCollection<ItemCompra> Itens => _itens.AsReadOnly();

    protected Compra() { }

    private Compra(Guid fornecedorId)
    {
        if (fornecedorId == Guid.Empty)
            throw new DomainException("Fornecedor inválido.");

        FornecedorId = fornecedorId;
        DataCompra = DateTime.UtcNow;
    }

    public static Compra CriarCompra(Guid fornecedorId)
        => new Compra(fornecedorId);

    public void AdicionarItem(Guid produtoId, int quantidade, decimal custoUnitario)
    {
        var item = new ItemCompra(produtoId, quantidade, custoUnitario);

        item.DefinirCompra(this.Id);

        _itens.Add(item);
    }

    public decimal TotalCompra => _itens.Sum(i => i.SubTotal);
}