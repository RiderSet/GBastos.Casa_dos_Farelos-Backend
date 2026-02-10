namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class ClientePJ : Pessoa
{
    public string CNPJ { get; private set; } = string.Empty;

    protected ClientePJ() { } // EF Core

    public ClientePJ(string nome, string email, string cnpj)
        : base(nome, email, cnpj) // CNPJ é o documento da Pessoa
    {
        SetCNPJ(cnpj);
    }

    public void AtualizarDados(string nome, string email)
    {
        Atualizar(nome, email, CNPJ); // método protegido da Pessoa
    }

    private void SetCNPJ(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("CNPJ é obrigatório");

        if (cnpj.Length != 14)
            throw new ArgumentException("CNPJ inválido");

        CNPJ = cnpj;
    }
}