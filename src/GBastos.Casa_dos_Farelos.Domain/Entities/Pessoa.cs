using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public abstract class Pessoa : Entity
{
    public string Nome { get; protected set; } = null!;
    public string Email { get; protected set; } = null!;
    public string Documento { get; protected set; } = null!;

    protected Pessoa() { } // EF

    protected Pessoa(string nome, string email, string documento)
    {
        Nome = nome;
        Email = email;
        Documento = documento;
    }

    protected void Atualizar(
        string nome,
        string email,
        string documento)
    {
        Nome = nome;
        Email = email;
        Documento = documento;
    }
}
