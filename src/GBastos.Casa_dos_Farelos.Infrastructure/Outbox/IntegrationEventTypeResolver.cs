using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using System.Reflection;

namespace GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

public sealed class IntegrationEventTypeResolver : IIntegrationEventTypeResolver
{
    private static readonly Dictionary<string, Type> _types;

    static IntegrationEventTypeResolver()
    {
        _types = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.Name.EndsWith("IntegrationEvent"))
            .ToDictionary(t => t.Name, t => t);
    }

    public Type? Resolve(string eventName)
        => _types.TryGetValue(eventName, out var type) ? type : null;
}