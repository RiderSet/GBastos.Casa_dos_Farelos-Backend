using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using System.Collections.Concurrent;

namespace GBastos.Casa_dos_Farelos.SharedKernel.Messaging;

public static class IntegrationEventRegistry
{
    private static readonly ConcurrentDictionary<string, Type> _registry = new();

    public static void Register<TEvent>()
        where TEvent : IIntegrationEvent
    {
        var eventType = typeof(TEvent);

        _registry.TryAdd(
            eventType.Name,
            eventType
        );
    }

    public static Type? GetEventType(string eventName)
    {
        _registry.TryGetValue(eventName, out var type);
        return type;
    }

    public static IEnumerable<Type> GetRegisteredEvents()
        => _registry.Values;
}