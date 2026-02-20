using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Events.Produto;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Produto : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string DescricaoProduto { get; private set; } = string.Empty;
    public decimal PrecoVenda { get; private set; }
    public int QuantEstoque { get; private set; }

    public Guid CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; } = null!;

    protected Produto() { }

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

    public void Atualizar(string nome, string descricao, decimal preco, Guid categoriaId, int quantEstoque)
    {
        AlterarNome(nome);
        AlterarDescricao(descricao);
        AlterarPreco(preco);
        CategoriaId = categoriaId;
        QuantEstoque = quantEstoque;
    }

    // ---------------- ESTOQUE ----------------
    public void AjustarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("A quantidade de entrada deve ser maior que zero.");

        checked
        {
            QuantEstoque += quantidade;
        }
    }

    public void BaixarEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        if (QuantEstoque < quantidade)
            throw new DomainException($"Estoque insuficiente do produto {Nome}");

        QuantEstoque -= quantidade;
    }

    private void AlterarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome do produto obrigatório.");

        Nome = nome.Trim();
    }

    private void AlterarDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição do produto é obrigatória.");

        DescricaoProduto = descricao.Trim();
    }

    private void AlterarPreco(decimal preco)
    {
        if (preco <= 0)
            throw new DomainException("Preço inválido.");

        PrecoVenda = preco;
    }
}