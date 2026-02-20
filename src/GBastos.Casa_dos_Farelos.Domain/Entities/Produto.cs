using GBastos.Casa_dos_Farelos.Domain.Common;
<<<<<<< HEAD
using GBastos.Casa_dos_Farelos.Domain.Events.Produto;
=======
using GBastos.Casa_dos_Farelos.Domain.Events;
>>>>>>> 532a5516c5422679921d3b0f6d7a9995a5d30bda

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Produto : BaseEntity
{
    public string Nome { get; private set; } = string.Empty;
    public string DescricaoProduto { get; private set; } = string.Empty;
    public decimal PrecoVenda { get; private set; }

    public int QuantEstoque { get; private set; }

    public Guid CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; } = null!;

<<<<<<< HEAD
    protected Produto() { }
=======
    public byte[] RowVersion { get; private set; } = null!;

    protected Produto() { } // EF
>>>>>>> 532a5516c5422679921d3b0f6d7a9995a5d30bda

    public Produto(string nome, string descricao, decimal precoVenda, Guid categoriaId, int estoqueInicial = 0)
    {
        Id = Guid.NewGuid();
        AlterarNome(nome);
        AlterarDescricao(descricao);
        AlterarPreco(precoVenda);

        CategoriaId = categoriaId;
        QuantEstoque = estoqueInicial;

        AddDomainEvent(new ProdutoCriadoEvent(nome, precoVenda));
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
            throw new DomainException($"Estoque insuficiente para o produto {Nome}");

        QuantEstoque -= quantidade;

        AddDomainEvent(new EstoqueBaixadoDomainEvent(
            Id,
            Nome,
            quantidade
        ));
    }

    private void AddDomainEvent(EstoqueBaixadoDomainEvent estoqueBaixadoDomainEvent)
    {
        throw new NotImplementedException();
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

    public void Atualizar(string nome, string descricao, decimal preco, Guid categoriaId, int quantEstoque)
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

    public void BaixarEstoque(int quantEstoque)
    {
        SaidaEstoque(quantEstoque);
    }
}