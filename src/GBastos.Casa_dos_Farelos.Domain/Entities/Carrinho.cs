using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Carrinho : BaseEntity
{
    public Guid ClienteId { get; private set; }
    public DateTime CriadoEm { get; private set; }

    public ICollection<CarrinhoItem> Itens { get; private set; } = new List<CarrinhoItem>();

    public decimal Total => Itens.Sum(i => i.PrecoUnitario * i.Quantidade);

    private Carrinho() { } // EF

    public Carrinho(Guid clienteId)
    {
        ClienteId = clienteId;
        CriadoEm = DateTime.UtcNow;
    }

    public void AdicionarItem(Guid produtoId, string nome, decimal precoUnitario, int quantidade = 1)
    {
        var itemExistente = Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        if (itemExistente != null)
            itemExistente.AdicionarQuantidade(quantidade);
        else
            Itens.Add(new CarrinhoItem(produtoId, nome, precoUnitario, quantidade));
    }

    public void RemoverItem(Guid produtoId)
    {
        var item = Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        if (item != null) Itens.Remove(item);
    }

    public void Limpar() => Itens.Clear();
}