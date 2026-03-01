using GBastos.Casa_dos_Farelos.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;

public abstract class Cliente : AggregateRoot<Guid>
{
    public string Nome { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    protected Cliente() : base(Guid.Empty) { } // EF Core

    protected Cliente(
        Guid id,
        string nome,
        string telefone,
        string email)
        : base(id)
    {
        SetNome(nome);
        SetTelefone(telefone);
        SetEmail(email);
    }

    private void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome obrigatório");

        Nome = nome.Trim();
    }

    private void SetTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
            throw new DomainException("Telefone obrigatório");

        Telefone = telefone.Trim();
    }

    private void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email obrigatório");

        Email = email.Trim();
    }
}