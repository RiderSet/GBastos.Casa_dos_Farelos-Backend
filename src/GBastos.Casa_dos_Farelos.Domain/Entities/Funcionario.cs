using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Enums;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Funcionario : Pessoa
{
    public string CPF { get; private set; } = null!;
    public Cargo Cargo { get; private set; }

    protected Funcionario() { }

    public Funcionario(
        string nome,
        string telefone,
        string email,
        string cpf,
        Cargo cargo
    ) : base(nome, telefone, email)
    {
        SetCpf(cpf);
        SetCargo(cargo);
    }

    public void Atualizar(
        string nome,
        string telefone,
        string email,
        string cpf,
        Cargo cargo)
    {
        base.Atualizar(nome, telefone, email);
        SetCpf(cpf);
        SetCargo(cargo);
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
        if (!Enum.IsDefined(cargo))
            throw new DomainException("Cargo inválido");

        Cargo = cargo;
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

        if (!Enum.IsDefined(typeof(Cargo), Cargo))
            throw new DomainException("Cargo do funcionário é inválido");
    }
}