using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Interfaces;

public interface IHasDomainEvents
{
    // Lista de eventos que a entidade acumulou
    List<DomainEvent> DomainEvents { get; }

    // Método para limpar os eventos depois de publicá-los
    void ClearDomainEvents();
}