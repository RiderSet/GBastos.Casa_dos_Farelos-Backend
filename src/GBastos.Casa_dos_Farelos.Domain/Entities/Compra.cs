using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Events.Compras;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Compra : AggregateRoot
{
    public DateTime DataCompra { get; private set; }
    public bool Finalizada { get; private set; }

    public Guid FuncionarioId { get; private set; }
    public Funcionario Funcionario { get; private set; } = null!;

    private readonly List<ItemCompra> _itens = new();
    public IReadOnlyCollection<ItemCompra> Itens => _itens.AsReadOnly();

    public decimal ValorTotal => _itens.Sum(i => i.SubTotal);

    protected Compra() { }

    private Compra(Guid funcionarioId)
    {
        Id = Guid.NewGuid();
        FuncionarioId = funcionarioId;
        DataCompra = DateTime.UtcNow;
        Finalizada = false;
    }

    public static Compra Criar(Guid funcionarioId)
        => new Compra(funcionarioId);

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

        ValidateInvariants();

        Finalizada = true;

        AddDomainEvent(new CompraCriadaDomainEvent(
            Id, // 🔥 CompraId
            FuncionarioId,
            ValorTotal,
            _itens.Select(i => new CompraItemSnapshot(
                i.ProdutoId,
                i.NomeProduto,
                i.Quantidade,
                i.CustoUnitario,
                i.SubTotal
            )).ToList()
        ));
    }

    public override void ValidateInvariants()
    {
        if (FuncionarioId == Guid.Empty)
            throw new DomainException("Funcionário é obrigatório.");

        if (DataCompra == default)
            throw new DomainException("Data da compra inválida.");

        if (Finalizada && !_itens.Any())
            throw new DomainException("Compra finalizada deve possuir itens.");

        foreach (var item in _itens)
        {
            if (item.ProdutoId == Guid.Empty)
                throw new DomainException("Item com produto inválido.");

            if (item.Quantidade <= 0)
                throw new DomainException("Item com quantidade inválida.");

            if (item.CustoUnitario <= 0)
                throw new DomainException("Item com custo unitário inválido.");
        }

        if (ValorTotal <= 0)
            throw new DomainException("Valor total inválido.");
    }
}