using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;
using GBastos.Casa_dos_Farelos.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Produto : AggregateRoot<Guid>
{
    public string Nome { get; private set; } = string.Empty;
    public string DescricaoProduto { get; private set; } = string.Empty;
    public decimal PrecoVenda { get; private set; }
    public int QuantEstoque { get; private set; }

    public Guid CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; } = null!;

    public byte[] RowVersion { get; private set; } = null!;

    protected Produto() : base(Guid.Empty) { }

    public Produto(
        string nome,
        string descricao,
        decimal precoVenda,
        Guid categoriaId,
        int estoqueInicial = 0)
        : base(Guid.NewGuid())
    {
        if (categoriaId == Guid.Empty)
            throw new DomainException("Categoria inválida.");

        AlterarNome(nome);
        AlterarDescricao(descricao);
        AlterarPreco(precoVenda);

        CategoriaId = categoriaId;
        QuantEstoque = estoqueInicial;

        AddDomainEvent(new ProdutoCriadoEvent(Id, Nome, PrecoVenda));
    }

    public void EntradaEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        checked
        {
            QuantEstoque += quantidade;
        }
    }

    public void SaidaEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        if (QuantEstoque < quantidade)
            throw new DomainException(
                $"Estoque insuficiente para o produto {Nome}");

        QuantEstoque -= quantidade;

        AddDomainEvent(new EstoqueBaixadoDomainEvent(
            Id,
            Nome,
            quantidade
        ));
    }

    public void Atualizar(
        string nome,
        string descricao,
        decimal preco,
        Guid categoriaId,
        int quantEstoque)
    {
        if (categoriaId == Guid.Empty)
            throw new DomainException("Categoria inválida.");

        if (quantEstoque < 0)
            throw new DomainException("Estoque não pode ser negativo.");

        AlterarNome(nome);
        AlterarDescricao(descricao);
        AlterarPreco(preco);

        CategoriaId = categoriaId;
        QuantEstoque = quantEstoque;
    }

    private void AlterarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome obrigatório.");

        Nome = nome.Trim();
    }

    private void AlterarDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição obrigatória.");

        DescricaoProduto = descricao.Trim();
    }

    private void AlterarPreco(decimal preco)
    {
        if (preco <= 0)
            throw new DomainException("Preço inválido.");

        PrecoVenda = preco;
    }
}