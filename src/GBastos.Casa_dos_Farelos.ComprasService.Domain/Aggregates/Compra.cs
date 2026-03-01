using GBastos.Casa_dos_Farelos.ComprasService.Domain.Entities;
using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;

namespace GBastos.Casa_dos_Farelos.ComprasService.Domain.Aggregates;

public class Compra : AggregateRoot<Guid>
{
    private readonly List<ItemCompra> _itens = new();

    public Compra(Guid id) : base(id)
    {
    }

    public IReadOnlyCollection<ItemCompra> Itens => _itens;

    public void AdicionarItem(
        Guid produtoId,
        string nomeProduto,
        int quantidade,
        decimal custoUnitario)
    {
        var item = new ItemCompra(
            Id,
            produtoId,
            nomeProduto,
            quantidade,
            custoUnitario);

        _itens.Add(item);
    }
}