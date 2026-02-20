using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Events.Compras;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Compra : AggregateRoot
{
    public Guid Id { get; private set; }
    public Guid FornecedorId { get; private set; }
    public DateTime DataCompra { get; private set; }
    public bool Finalizada { get; private set; }

    public Guid FuncionarioId { get; private set; }
    public Funcionario Funcionario { get; private set; } = null!;

    private readonly List<ItemCompra> _itens = new();
    public IReadOnlyCollection<ItemCompra> Itens => _itens;

    public decimal ValorTotal => _itens.Sum(i => i.SubTotal);

    protected Compra() { }

    private Compra(Guid fornecedorId, Guid funcionarioId)
    {
        if (fornecedorId == Guid.Empty)
            throw new DomainException("Fornecedor inválido.");

        if (funcionarioId == Guid.Empty)
            throw new DomainException("Funcionário inválido.");

        Id = Guid.NewGuid();
        FornecedorId = fornecedorId;
        FuncionarioId = funcionarioId;
        DataCompra = DateTime.UtcNow;
        Finalizada = false;
    }

    public static Compra Criar(Guid fornecedorId, Guid funcionarioId)
        => new Compra(fornecedorId, funcionarioId);

    public void AdicionarProduto(Guid produtoId, string nomeProduto, int quantidade, decimal custoUnitario)
    {
        if (Finalizada)
            throw new DomainException("Compra já finalizada.");

        var item = new ItemCompra(produtoId, nomeProduto, quantidade, custoUnitario);
        item.DefinirCompra(Id);

        _itens.Add(item);
    }

    public void Finalizar()
    {
        if (Finalizada)
            throw new DomainException("Compra já finalizada.");

        if (!_itens.Any())
            throw new DomainException("Compra deve possuir itens.");

        var nomeFuncionario = Funcionario?.Nome
    ?? throw new DomainException("Funcionário não carregado.");

        Finalizada = true;

        // 🔥 Conciliação com estoque via evento
        var snapshot = _itens.Select(i =>
            new CompraItemSnapshot(
                i.ProdutoId,
                i.NomeProduto,
                i.Quantidade,
                i.CustoUnitario,
                i.SubTotal
            )).ToList();

        AddDomainEvent(new CompraCriadaDomainEvent(
            Id,
            FornecedorId,
            FuncionarioId,
            nomeFuncionario,
            DataCompra,
            ValorTotal,
            snapshot
        ));
    }

    protected override void ValidateInvariants()
    {
        if (FornecedorId == Guid.Empty)
            throw new DomainException("Fornecedor inválido.");

        if (FuncionarioId == Guid.Empty)
            throw new DomainException("Funcionário inválido.");

        if (Finalizada && !_itens.Any())
            throw new DomainException("Compra finalizada deve possuir itens.");
    }
}