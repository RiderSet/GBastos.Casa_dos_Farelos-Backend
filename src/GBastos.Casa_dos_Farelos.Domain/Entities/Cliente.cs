namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public abstract class Cliente : Pessoa
{
    protected Cliente() { }

    protected Cliente(string nome, string telefone, string email)
    {
        CriarCliente(nome, telefone, email);
    }

    protected void CriarCliente(string nome, string telefone, string email)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório", nameof(nome));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório", nameof(email));

        Nome = nome.Trim();
        SetTelefone(telefone);
        Email = email.Trim().ToLowerInvariant();
        DtCadastro = DateTime.UtcNow;
    }
}