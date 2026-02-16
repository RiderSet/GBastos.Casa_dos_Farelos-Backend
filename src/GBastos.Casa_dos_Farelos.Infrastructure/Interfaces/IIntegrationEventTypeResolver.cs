namespace GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;

public interface IIntegrationEventTypeResolver
{
    Type? Resolve(string eventName);
}