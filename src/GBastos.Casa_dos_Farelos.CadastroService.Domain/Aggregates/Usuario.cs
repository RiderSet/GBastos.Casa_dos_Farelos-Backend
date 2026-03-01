using GBastos.Casa_dos_Farelos.SharedKernel.Abstractions;

namespace GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;

internal class Usuario : AggregateRoot<Guid>
{
    public Usuario(Guid id) : base(id)
    {
    }
}
