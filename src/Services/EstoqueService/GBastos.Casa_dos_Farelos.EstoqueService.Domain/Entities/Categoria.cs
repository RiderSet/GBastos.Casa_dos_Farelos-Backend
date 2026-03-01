using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;
using GBastos.Casa_dos_Farelos.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Entities;

public class Categoria : Entity<Guid>
{
    public string Nome { get; private set; } = string.Empty;

    protected Categoria() : base(Guid.Empty) { }

    public Categoria(string nome)
        : base(Guid.NewGuid())
    {
        AlterarNome(nome);
    }

    public void AlterarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("Nome da categoria é obrigatório.");

        Nome = nome.Trim();
    }
}