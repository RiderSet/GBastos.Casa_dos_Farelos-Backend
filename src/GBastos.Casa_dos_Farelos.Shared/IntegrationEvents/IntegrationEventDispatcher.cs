using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

public static class IntegrationEventDispatcher
{
    public static async Task DispatchAsync(
        IServiceProvider provider,
        IIntegrationEventTypeResolver typeResolver, 
        string payload,
        string eventType,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(payload))
            throw new ArgumentNullException(nameof(payload));
        if (string.IsNullOrWhiteSpace(eventType))
            throw new ArgumentNullException(nameof(eventType));
        if (typeResolver == null)
            throw new ArgumentNullException(nameof(typeResolver));

        // Resolve o tipo do evento via resolver
        var type = typeResolver.Resolve(eventType);

        // Desserializa o payload
        var evt = (IIntegrationEvent)JsonSerializer.Deserialize(payload, type)!;

        // Resolve os handlers registrados
        var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(type);
        var handlers = provider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var method = handlerType.GetMethod(nameof(IIntegrationEventHandler<IIntegrationEvent>.HandleAsync))!;
            await (Task)method.Invoke(handler, new object[] { evt, ct })!;
        }
    }
}