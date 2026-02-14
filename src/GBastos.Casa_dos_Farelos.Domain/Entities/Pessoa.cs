using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public abstract class Pessoa : BaseEntity
{
    public string Nome { get; protected set; } = null!;
    public string Email { get; protected set; } = null!;
    public string Telefone { get; private set; } = null!;
    public DateTime DtCadastro { get; private set; }

    protected Pessoa() { } // EF

    protected Pessoa(string nome, string telefone, string email)
    {
        SetNome(nome);
        SetTelefone(telefone);
        SetEmail(email);
        DtCadastro = DateTime.UtcNow;
    }

    public void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome é obrigatório");

        Nome = nome.Trim();
    }

    public void SetTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
            throw new DomainException("Telefone não informado.");

        Telefone = telefone.Trim();
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email obrigatório");

        Email = email.Trim().ToLower();
    }

    public void SetDtCadastro(DateTime dtCadastro)
    {
        if (string.IsNullOrWhiteSpace(dtCadastro.ToString()))
            throw new DomainException("Email obrigatório");

        DtCadastro = dtCadastro;
    }

    public virtual void Atualizar(string nome, string telefone, string email, DateTime dtCadastro)
    {
        SetNome(nome);
        SetTelefone(telefone);
        SetEmail(email);
        SetDtCadastro(dtCadastro);
    }
}