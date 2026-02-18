using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public abstract class Cliente : AggregateRoot
{
    public string Nome { get; protected set; } = string.Empty;
    public string Telefone { get; protected set; } = string.Empty;
    public string Email { get; protected set; } = string.Empty;
    public DateTime DtCadastro { get; private set; }

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
        Telefone = telefone?.Trim() ?? "";
        Email = email.Trim().ToLowerInvariant();
        DtCadastro = DateTime.UtcNow;
    }

    public void Atualizar(string nome, string telefone, string email)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório", nameof(nome));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email é obrigatório", nameof(email));

        Nome = nome.Trim();
        Telefone = telefone?.Trim() ?? "";
        Email = email.Trim().ToLowerInvariant();
    }
}