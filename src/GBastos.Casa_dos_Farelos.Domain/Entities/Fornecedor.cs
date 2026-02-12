using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Fornecedor : Pessoa
{
    public string CNPJ { get; private set; } = null!;

    private readonly List<Produto> _produtos = new();
    public IReadOnlyCollection<Produto> Produtos => _produtos.AsReadOnly();

    private Fornecedor() { }

    public Fornecedor(string nome, string telefone, string email, string cnpj, DateTime DtCadastro)
        : base(nome, telefone, email)
    {
        SetCnpj(cnpj);
    }

    public void Atualizar(string nome, string telefone, string email, string cnpj, DateTime DtCadastro)
    {
        base.Atualizar(nome, telefone, email, DtCadastro);
        SetCnpj(cnpj);
    }

    private void SetCnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new DomainException("CNPJ é obrigatório");

        cnpj = SomenteNumeros(cnpj);

        if (cnpj.Length != 14)
            throw new DomainException("CNPJ inválido");

        CNPJ = cnpj;
    }

    public void AdicionarProduto(Produto produto)
    {
        if (produto is null)
            throw new DomainException("Produto inválido");

        if (_produtos.Any(p => p.Id == produto.Id))
            throw new DomainException("Produto já vinculado ao fornecedor");

        _produtos.Add(produto);
    }

    public void RemoverProduto(Guid produtoId)
    {
        var produto = _produtos.FirstOrDefault(p => p.Id == produtoId);

        if (produto is null)
            throw new DomainException("Produto não pertence ao fornecedor");

        _produtos.Remove(produto);
    }

    private static string SomenteNumeros(string valor)
        => new string(valor.Where(char.IsDigit).ToArray());
}