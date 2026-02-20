namespace GBastos.Casa_dos_Farelos.Domain.Dtos;

public sealed class ProdutoDto
{
    public Guid Id { get; init; }
    public string Nome { get; init; } = string.Empty;
    public string Descricao { get; init; } = string.Empty;
    public decimal Preco { get; init; }
    public int Estoque { get; init; }

    // Construtor opcional para facilitar mapeamento manual
    public ProdutoDto(Guid id, string nome, string descricao, decimal preco, int estoque)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        Estoque = estoque;
    }
}
