namespace GBastos.Casa_dos_Farelos.Shared.Interfaces;

public interface IIntegrationEventTypeResolver
{
    Type? Resolve(string eventName);
}