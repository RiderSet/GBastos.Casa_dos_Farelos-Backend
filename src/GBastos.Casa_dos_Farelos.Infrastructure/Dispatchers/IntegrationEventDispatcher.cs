using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Dispatchers;

public static class IntegrationEventDispatcher
{
    public static async Task DispatchAsync(
        IServiceProvider provider,
        string payload,
        string eventType,
        CancellationToken ct)
    {
        var type = Type.GetType(eventType)!;

        var evt = (IIntegrationEvent)JsonSerializer.Deserialize(payload, type)!;

        var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(type);
        var handlers = provider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var method = handlerType.GetMethod(nameof(IIntegrationEventHandler<IIntegrationEvent>.HandleAsync))!;
            await (Task)method.Invoke(handler, new object[] { evt, ct })!;
        }
    }
}