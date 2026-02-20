using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Estoque;

public class MovimentacaoEstoque : BaseEntity
{
    public Guid ProdutoId { get; private set; }
    public string NomeProduto { get; private set; } = null!;
    public int Quantidade { get; private set; }
    public string Tipo { get; private set; } = null!;
    public DateTime DataMovimentacao { get; private set; }

    protected MovimentacaoEstoque() { }

    public MovimentacaoEstoque(Guid produtoId, string nomeProduto, int quantidade, string tipo)
    {
        ProdutoId = produtoId;
        NomeProduto = nomeProduto;
        Quantidade = quantidade;
        Tipo = tipo;
        DataMovimentacao = DateTime.UtcNow;
    }
}