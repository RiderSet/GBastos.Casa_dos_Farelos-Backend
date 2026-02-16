using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Outbox;

public sealed class EventTypeResolver : IEventTypeResolver
{
    private static readonly Dictionary<string, Type> _map;

    static EventTypeResolver()
    {
        _map = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.Name.EndsWith("DomainEvent"))
            .ToDictionary(t => t.Name, t => t);
    }

    public Type? Resolve(string eventName)
        => _map.TryGetValue(eventName, out var type) ? type : null;
}