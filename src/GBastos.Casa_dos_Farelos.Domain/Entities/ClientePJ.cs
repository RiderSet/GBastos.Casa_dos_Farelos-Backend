using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public sealed class ClientePJ : Pessoa
{
    public string CNPJ { get; private set; } = null!;
    public string Contato { get; private set; } = null!;

    private ClientePJ() { } // EF

    public ClientePJ(string nome, string telefone, string email, string cnpj, string contato)
        : base(nome, telefone, email)
    {
        SetCnpj(cnpj);
        SetContato(contato);
    }

    public void Atualizar(string nome, string telefone, string email, string contato, DateTime dtCadastro)
    {
        base.Atualizar(nome, telefone, email, dtCadastro);
        SetContato(contato);
    }

    private void SetCnpj(string cnpj)
    {
        if (CNPJ != null)
            throw new DomainException("CNPJ não pode ser alterado");

        if (string.IsNullOrWhiteSpace(cnpj))
            throw new DomainException("CNPJ obrigatório");

        cnpj = SomenteNumeros(cnpj);

        if (cnpj.Length != 14)
            throw new DomainException("CNPJ inválido");

        CNPJ = cnpj;
    }

    private void SetContato(string contato)
    {
        if (string.IsNullOrWhiteSpace(contato))
            throw new DomainException("Contato obrigatório");

        Contato = contato.Trim();
    }

    private static string SomenteNumeros(string valor)
        => new string(valor.Where(char.IsDigit).ToArray());
}