using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public sealed class ClientePF : Pessoa
{
    public string CPF { get; private set; } = null!;
    public DateTime? DtNascimento { get; private set; }

    private ClientePF() { }

    public ClientePF(string nome, string telefone, string email, string cpf, DateTime dtCadastro)
        : base(nome, telefone, email)
    {
        SetCpf(cpf);
    }

    public void Atualizar(string nome, string telefone, string email, string cpf, DateTime dtCadastro)
    {
        base.Atualizar(nome, telefone, email, dtCadastro);
        SetCpf(cpf);
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

    private static string SomenteNumeros(string valor)
        => new string(valor.Where(char.IsDigit).ToArray());
}