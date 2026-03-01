using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.SharedKernel.Events;
using GBastos.Casa_dos_Farelos.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;

public class Fornecedor : AggregateRoot<Guid>
{
    public string Nome { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string CNPJ { get; private set; } = string.Empty;

    private readonly List<Produto> _produtos = new();
    public IReadOnlyCollection<Produto> Produtos => _produtos.AsReadOnly();

    private Fornecedor() : base(Guid.Empty) { }

    private Fornecedor(
        Guid id,
        string nome,
        string telefone,
        string email,
        string cnpj)
        : base(id)
    {
        AtualizarDadosBasicos(nome, telefone, email);
        SetCnpj(cnpj);

        ValidateInvariants();

        AddDomainEvent(
            new FornecedorAtualizadoDomainEvent(
                Id,
                Nome,
                Telefone,
                Email,
                CNPJ
            )
        );
    }

    public static Fornecedor Criar(
        string nome,
        string telefone,
        string email,
        string cnpj)
    {
        return new Fornecedor(
            Guid.NewGuid(),
            nome,
            telefone,
            email,
            cnpj);
    }

    #region Update Domain

    public void Atualizar(
        string nome,
        string telefone,
        string email,
        string cnpj)
    {
        AtualizarDadosBasicos(nome, telefone, email);
        SetCnpj(cnpj);

        ValidateInvariants();

        AddDomainEvent(
            new FornecedorAtualizadoDomainEvent(
                Id,
                Nome,
                Telefone,
                Email,
                CNPJ
            )
        );
    }

    private void AtualizarDadosBasicos(
        string nome,
        string telefone,
        string email)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome obrigatório");

        if (string.IsNullOrWhiteSpace(telefone))
            throw new DomainException("Telefone obrigatório");

        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email obrigatório");

        Nome = nome.Trim();
        Telefone = telefone.Trim();
        Email = email.Trim();
    }

    #endregion

    #region CNPJ Rules

    private void SetCnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new DomainException("CNPJ é obrigatório");

        cnpj = SomenteNumeros(cnpj);

        if (cnpj.Length != 14)
            throw new DomainException("CNPJ inválido");

        CNPJ = cnpj;
    }

    private static string SomenteNumeros(string valor)
        => new(valor.Where(char.IsDigit).ToArray());

    #endregion

    #region Produtos

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

    #endregion

    protected override void ValidateInvariants()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new DomainException("Nome do fornecedor é obrigatório");

        if (string.IsNullOrWhiteSpace(Telefone))
            throw new DomainException("Telefone do fornecedor é obrigatório");

        if (string.IsNullOrWhiteSpace(Email))
            throw new DomainException("Email do fornecedor é obrigatório");

        if (string.IsNullOrWhiteSpace(CNPJ) || CNPJ.Length != 14)
            throw new DomainException("CNPJ do fornecedor é inválido");
    }
}