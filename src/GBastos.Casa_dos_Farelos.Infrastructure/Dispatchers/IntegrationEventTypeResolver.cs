using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using System.Reflection;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Dispatchers;
public sealed class IntegrationEventTypeResolver : IIntegrationEventTypeResolver
{
    private static readonly Dictionary<string, Type> _types;

    static IntegrationEventTypeResolver()
    {
        _types = Assembly
            .GetAssembly(typeof(IIntegrationEvent))!
            .GetTypes()
            .Where(t => typeof(IIntegrationEvent).IsAssignableFrom(t) && !t.IsAbstract)
            .ToDictionary(t => t.FullName!, t => t);
    }

    public Type? Resolve(string eventTypeName)
        => _types.GetValueOrDefault(eventTypeName);
}