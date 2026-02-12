using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Enums;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Funcionario : Pessoa
{
    public string CPF { get; private set; } = null!;
    public Cargo Cargo { get; private set; }

    protected Funcionario() { } // EF Core

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
        base.Atualizar(nome, telefone, email, DtCadastro);
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
}