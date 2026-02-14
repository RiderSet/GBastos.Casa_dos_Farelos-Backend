using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public sealed class ClientePF : Pessoa
{
    public string CPF { get; private set; }
    public string Telefone { get; private set; }
    public DateTime DtCadastro { get; private set; }

    private ClientePF() { }

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
            Telefone = telefone,
            Email = email,
            CPF = cpf,
            DtCadastro = DateTime.UtcNow
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
        Telefone = telefone;
        Email = email;
        CPF = cpf;
        DtCadastro = dtCadastro;
    }
}