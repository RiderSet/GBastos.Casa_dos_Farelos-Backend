namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public sealed class ClientePF : Pessoa
{
    public string CPF { get; private set; } = string.Empty;
    public string Telefone { get; private set; } = string.Empty;
    public DateTime DataNascimento { get; private set; }
    public DateTime CriadoEm { get; private set; }

    private ClientePF() { } // EF

    public ClientePF(string nome, string email, string cpf)
        : base(nome, email, cpf) // cpf é o documento da Pessoa
    {
        SetCpf(cpf);
    }

    public ClientePF(string nome, string cpf, string telefone, string email, DateTime dataNascimento)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        CPF = cpf;
        Telefone = telefone;
        Email = email;
        DataNascimento = dataNascimento;
        CriadoEm = DateTime.UtcNow;
    }

    public void AtualizarDados(string nome, string cpf, string telefone, string email, DateTime dataNascimento)
    {
        Nome = nome;
        Telefone = telefone;
        CPF = cpf;
        Email = email;
        DataNascimento = dataNascimento;
    }

    private void SetCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF é obrigatório");

        if (cpf.Length != 11)
            throw new ArgumentException("CPF inválido");

        CPF = cpf;
    }
}