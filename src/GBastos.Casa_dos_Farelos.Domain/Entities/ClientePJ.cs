namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public sealed class ClientePJ : Pessoa
{
    public string CNPJ { get; private set; } = string.Empty;
    public string NomeFantasia { get; private set; } = string.Empty;
    public string Contato { get; private set; } = string.Empty;

    // EF Core
    private ClientePJ() { }

    private ClientePJ(
        string nome,
        string telefone,
        string email,
        string nomeFantasia,
        string cnpj,
        string contato)
        : base(nome, telefone, email)
    {
        DefinirDadosEmpresa(nomeFantasia, cnpj, contato);
    }

    public static ClientePJ CriarClientePJ(
        string nomeFantasia,
        string telefone,
        string email,
        string cnpj,
        string contato)
    {
        return new ClientePJ(nomeFantasia, telefone, email, nomeFantasia, cnpj, contato);
    }

    public void AtualizarClientePJ(
        string nome,
        string telefone,
        string email,
        string nomeFantasia,
        string cnpj,
        string contato)
    {
        Atualizar(nome, telefone, email); // método da Pessoa
        DefinirDadosEmpresa(nomeFantasia, cnpj, contato);
    }

    private void DefinirDadosEmpresa(string nomeFantasia, string cnpj, string contato)
    {
        if (string.IsNullOrWhiteSpace(nomeFantasia))
            throw new ArgumentException("Nome fantasia é obrigatório", nameof(nomeFantasia));

        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("CNPJ é obrigatório", nameof(cnpj));

        if (cnpj.Length != 14)
            throw new ArgumentException("CNPJ inválido", nameof(cnpj));

        NomeFantasia = nomeFantasia;
        CNPJ = cnpj;
        Contato = contato;
    }
}