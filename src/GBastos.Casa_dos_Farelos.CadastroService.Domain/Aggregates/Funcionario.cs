using GBastos.Casa_dos_Farelos.CadastroService.Domain.Entities;
using GBastos.Casa_dos_Farelos.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;

public class Funcionario : AggregateRoot<Guid>
{
    public string Nome { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string CPF { get; private set; } = string.Empty;

    public Guid CargoId { get; private set; }
    public Cargo Cargo { get; private set; } = null!;

    protected Funcionario() : base(Guid.Empty) { }

    public Funcionario(
        string nome,
        string telefone,
        string email,
        string cpf,
        Cargo cargo)
        : base(Guid.NewGuid())
    {
        SetNome(nome);
        SetTelefone(telefone);
        SetEmail(email);
        SetCpf(cpf);
        SetCargo(cargo);

        ValidateInvariants();
    }

    public void Atualizar(
        string nome,
        string telefone,
        string email,
        string cpf,
        Cargo cargo)
    {
        SetNome(nome);
        SetTelefone(telefone);
        SetEmail(email);
        SetCpf(cpf);
        SetCargo(cargo);

        ValidateInvariants();
    }

    private void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome é obrigatório");

        Nome = nome.Trim();
    }

    private void SetTelefone(string telefone)
    {
        if (string.IsNullOrWhiteSpace(telefone))
            throw new DomainException("Telefone é obrigatório");

        Telefone = telefone.Trim();
    }

    private void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email é obrigatório");

        Email = email.Trim();
    }

    private void SetCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new DomainException("CPF é obrigatório");

        cpf = SomenteNumeros(cpf);

        if (cpf.Length != 11)
            throw new DomainException("CPF inválido");

        CPF = cpf;
    }

    private void SetCargo(Cargo cargo)
    {
        if (cargo is null)
            throw new DomainException("Cargo é obrigatório");

        if (!cargo.Ativo)
            throw new DomainException("Cargo está inativo");

        Cargo = cargo;
        CargoId = cargo.Id;
    }

    private static string SomenteNumeros(string valor)
        => new string(valor.Where(char.IsDigit).ToArray());

    protected override void ValidateInvariants()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new DomainException("Nome do funcionário é obrigatório");

        if (string.IsNullOrWhiteSpace(Telefone))
            throw new DomainException("Telefone do funcionário é obrigatório");

        if (string.IsNullOrWhiteSpace(Email))
            throw new DomainException("Email do funcionário é obrigatório");

        if (string.IsNullOrWhiteSpace(CPF) || CPF.Length != 11)
            throw new DomainException("CPF do funcionário é inválido");

        if (CargoId == Guid.Empty)
            throw new DomainException("Cargo do funcionário é obrigatório");
    }
}