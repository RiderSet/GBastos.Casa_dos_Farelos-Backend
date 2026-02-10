using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Venda : Entity
{
    public Guid ClienteId { get; private set; }
    public Guid FuncionarioId { get; private set; }

    public DateTime DataVenda { get; private set; }

    private readonly List<ItemVenda> _itens = new();
    public IReadOnlyCollection<ItemVenda> Itens => _itens.AsReadOnly();

    public decimal TotalVenda => _itens.Sum(i => i.SubTotal);

    public decimal TaxaEntrega { get; private set; }

    protected Venda() { }

    private Venda(Guid clienteId, Guid funcionarioId)
    {
        if (clienteId == Guid.Empty)
            throw new DomainException("Cliente inválido.");

        if (funcionarioId == Guid.Empty)
            throw new DomainException("Funcionário inválido.");

        ClienteId = clienteId;
        FuncionarioId = funcionarioId;
        DataVenda = DateTime.UtcNow;
    }

    public static Venda Criar(Guid clienteId, Guid funcionarioId, IEnumerable<ItemVenda> itens)
    {
        var venda = new Venda(clienteId, funcionarioId);

        foreach (var item in itens)
            venda.AdicionarItem(item);

        if (!venda._itens.Any())
            throw new DomainException("Venda deve possuir ao menos um item.");

        return venda;
    }

    private void AdicionarItem(ItemVenda item)
    {
        if (item == null)
            throw new DomainException("Item inválido.");

        item.DefinirVenda(Id);
        _itens.Add(item);
    }

    public void AplicarEntrega()
    {
        TaxaEntrega = TotalVenda * 0.10m;
    }
}