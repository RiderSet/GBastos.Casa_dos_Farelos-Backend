namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public sealed class ClientePJ : Pessoa
{
    public string CNPJ { get; private set; } = string.Empty;
    public string NomeFantasia { get; private set; } = string.Empty;
    public string Contato { get; private set; } = string.Empty;

    private ClientePJ() { }

    public static ClientePJ CriarClientePJ(
        string nomeFantasia,
        string telefone,
        string email,
        string cnpj,
        string contato)
    {
        if (string.IsNullOrWhiteSpace(nomeFantasia))
            throw new ArgumentException("Nome fantasia é obrigatório", nameof(nomeFantasia));
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("CNPJ é obrigatório", nameof(cnpj));

        var cliente = new ClientePJ
        {
            Id = Guid.NewGuid(),
            NomeFantasia = nomeFantasia,
            Email = email,
            CNPJ = cnpj,
            Contato = contato
        };

        return cliente;
    }

    public void AtualizarClentePJ(
        string nomeFantasia,
        string telefone,
        string email,
        string cnpj,
        string contato,
        DateTime dtCadastro)
    {
        NomeFantasia = nomeFantasia;
        Email = email;
        CNPJ = cnpj;
        Contato = contato;
    }
}