namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;

public class ProdutoEstoque
{
    public Guid Id { get; private set; }
    public Guid ProdutoId { get; private set; }
    public int QuantidadeDisponivel { get; private set; }

    private ProdutoEstoque() { }

    public void Debitar(int quantidade)
    {
        if (quantidade > QuantidadeDisponivel)
            throw new Exception("Estoque insuficiente");

        QuantidadeDisponivel -= quantidade;
    }

    public void Repor(int quantidade)
    {
        QuantidadeDisponivel += quantidade;
    }
}