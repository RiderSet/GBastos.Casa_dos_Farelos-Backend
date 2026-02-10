using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Categoria : Entity
{
    public string Nome { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public bool Ativa { get; private set; }

    // BACKING FIELD
    private readonly List<Produto> _produtos = new();
    public IReadOnlyCollection<Produto> Produtos => _produtos.AsReadOnly();

    protected Categoria() { } // EF

    public Categoria(string nome, string descricao)
    {
        AlterarNome(nome);
        AlterarDescricao(descricao);
        Ativar();
    }

    public void Atualizar(string nome, string descricao)
    {
        AlterarNome(nome);
        AlterarDescricao(descricao);
    }

    public void Ativar() => Ativa = true;
    public void Desativar() => Ativa = false;

    private void AlterarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome da categoria é obrigatório.");
        if (nome.Length < 2)
            throw new DomainException("Nome da categoria muito curto.");

        Nome = nome.Trim();
    }

    private void AlterarDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição da categoria é obrigatória.");

        Descricao = descricao.Trim();
    }

    internal void AdicionarProduto(Produto produto)
    {
        if (produto == null) throw new DomainException("Produto inválido.");
        _produtos.Add(produto);
    }
}