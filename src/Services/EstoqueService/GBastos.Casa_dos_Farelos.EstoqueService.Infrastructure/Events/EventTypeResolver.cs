using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using System.Collections.Concurrent;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Events;

public sealed class EventTypeResolver : IEventTypeResolver
{
    private readonly ConcurrentDictionary<string, Type> _cache;

    public EventTypeResolver()
    {
        _cache = new ConcurrentDictionary<string, Type>();

        PreloadDomainEvents();
    }

    private void PreloadDomainEvents()
    {
        var eventTypes = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic)
            .SelectMany(a =>
            {
                try
                {
                    return a.GetTypes();
                }
                catch
                {
                    return Enumerable.Empty<Type>();
                }
            })
            .Where(t =>
                typeof(IDomainEvent).IsAssignableFrom(t) &&
                !t.IsInterface &&
                !t.IsAbstract);

        foreach (var type in eventTypes)
        {
            _cache.TryAdd(type.FullName!, type);
        }
    }

    public Type? Resolve(string eventTypeName)
    {
        _cache.TryGetValue(eventTypeName, out var type);
        return type;
    }
}