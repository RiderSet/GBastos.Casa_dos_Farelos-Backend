using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Dispatchers;

public sealed class IntegrationEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public IntegrationEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(IIntegrationEvent evt, CancellationToken ct)
    {
        var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(evt.GetType());
        var handlers = _serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var method = handlerType.GetMethod("HandleAsync")!;
            await (Task)method.Invoke(handler, new object[] { evt, ct })!;
        }
    }

    public async Task DispatchAsync(string payload, Type eventType, CancellationToken ct)
    {
        var evt = (IIntegrationEvent)JsonSerializer.Deserialize(payload, eventType)!;
        await DispatchAsync(evt, ct);
    }
}