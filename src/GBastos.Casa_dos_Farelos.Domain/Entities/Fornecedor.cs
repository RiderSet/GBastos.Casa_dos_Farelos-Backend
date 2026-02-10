namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Fornecedor : Pessoa
{
    public List<Produto> Produtos { get; private set; } = new();

    public IReadOnlyCollection<Produto> _produtos => Produtos;

    public Fornecedor(string nome) => Nome = nome;

    private Fornecedor() { }

    public Fornecedor(
        string nome,
        string email,
        string documento,
        IEnumerable<Produto> produtos)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Email = email;
        Documento = documento;
        Produtos.AddRange(produtos);
    }

    public void AdicionarProduto(Produto produto)
    => Produtos.Add(produto);

    public Fornecedor UpdateName(string nome)
    {
        return new Fornecedor(
            nome: Nome,
            email: Email,
            documento: Documento,
            produtos: Produtos
        );
    }
}