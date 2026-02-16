namespace GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;

public interface IEventTypeResolver
{
    Type? Resolve(string eventName);
}