using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Shared.Events;

public sealed class InMemoryEventBus : IEventBus
{
    private readonly IServiceProvider _provider;

    public InMemoryEventBus(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task Publish<T>(T @event, CancellationToken ct = default) where T : class
    {
        using var scope = _provider.CreateScope();

        var handlers = scope.ServiceProvider
            .GetServices<IEventHandler<T>>();

        foreach (var handler in handlers)
            await handler.Handle(@event, ct);
    }

    public async Task Publish(object @event, CancellationToken ct = default)
    {
        var eventType = @event.GetType();

        var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);
        var handlers = _provider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var method = handlerType.GetMethod("Handle")!;
            var task = (Task)method.Invoke(handler, new[] { @event, ct })!;
            await task;
        }
    }
}
