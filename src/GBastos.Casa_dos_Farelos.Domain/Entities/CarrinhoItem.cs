namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class CarrinhoItem
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProdutoId { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public decimal PrecoUnitario { get; private set; }
    public int Quantidade { get; private set; }

    private CarrinhoItem() { } // EF

    public CarrinhoItem(Guid produtoId, string nome, decimal precoUnitario, int quantidade)
    {
        ProdutoId = produtoId;
        Nome = nome;
        PrecoUnitario = precoUnitario;
        Quantidade = quantidade;
    }

    public void AdicionarQuantidade(int quantidade) => Quantidade += quantidade;
    public void AtualizarQuantidade(int quantidade) => Quantidade = quantidade;
}