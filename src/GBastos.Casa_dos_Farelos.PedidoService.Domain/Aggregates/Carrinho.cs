using GBastos.Casa_dos_Farelos.PedidoService.Domain.Entities;
using GBastos.Casa_dos_Farelos.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Aggregates;

public class Carrinho : AggregateRoot<Guid>
{
    public Guid ClienteId { get; private set; }
    public DateTime CriadoEm { get; private set; }

    private readonly List<CarrinhoItem> _itens = new();
    public IReadOnlyCollection<CarrinhoItem> Itens => _itens;

    public decimal Total => _itens.Sum(i => i.PrecoUnitario * i.Quantidade);

    protected Carrinho() : base(Guid.Empty) { } // EF

    public Carrinho(Guid clienteId) : base(Guid.NewGuid())
    {
        if (clienteId == Guid.Empty)
            throw new DomainException("Cliente inválido.");

        ClienteId = clienteId;
        CriadoEm = DateTime.UtcNow;
    }

    public void AdicionarItem(
        Guid produtoId,
        string nome,
        decimal precoUnitario,
        int quantidade = 1)
    {
        if (produtoId == Guid.Empty)
            throw new DomainException("Produto inválido.");

        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do produto obrigatório.");

        if (precoUnitario <= 0)
            throw new DomainException("Preço inválido.");

        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        var itemExistente = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);

        if (itemExistente != null)
            itemExistente.AdicionarQuantidade(quantidade);
        else
            _itens.Add(new CarrinhoItem(produtoId, nome, precoUnitario, quantidade));
    }

    public void RemoverItem(Guid produtoId)
    {
        var item = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);

        if (item != null)
            _itens.Remove(item);
    }

    public void Limpar() => _itens.Clear();
}