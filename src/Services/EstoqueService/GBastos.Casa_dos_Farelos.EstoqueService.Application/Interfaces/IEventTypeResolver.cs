namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public interface IEventTypeResolver
{
    Type? Resolve(string eventTypeName);
}
