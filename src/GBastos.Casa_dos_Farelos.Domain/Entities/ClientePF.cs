namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public sealed class ClientePF : Pessoa
{
    private string v;

    public string CPF { get; private set; } = string.Empty;
    public DateTime DtNascimento { get; private set; }

    private ClientePF() { }

    public ClientePF(string nome, string telefone, string email, string v) : base(nome, telefone, email)
    {
    }

    public static ClientePF CriarClientePF(
        string nome,
        string telefone,
        string email,
        string cpf)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório", nameof(nome));
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF é obrigatório", nameof(cpf));

        var cliente = new ClientePF
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Email = email,
            CPF = cpf
        };

        return cliente;
    }

    public void AtualizarClientePF(
        string nome,
        string telefone,
        string email,
        string cpf,
        DateTime dtCadastro)
    {
        Nome = nome;
        Email = email;
        CPF = cpf;
    }
}