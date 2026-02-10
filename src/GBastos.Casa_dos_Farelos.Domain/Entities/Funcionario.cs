using GBastos.Casa_dos_Farelos.Domain.Enums;

namespace GBastos.Casa_dos_Farelos.Domain.Entities;

public class Funcionario : Pessoa
{
    public Cargo Cargo { get; private set; }

    protected Funcionario() { }

    public Funcionario(
        string nome,
        string email,
        string documento,
        Cargo cargo
    )
    {
        Nome = nome;
        Email = email;
        Documento = documento;
        Cargo = cargo;
    }
}
