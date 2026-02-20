using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GBastos.Casa_dos_Farelos.Shared.Events.Compras;

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
        var handlers = scope.ServiceProvider.GetServices<IEventHandler<T>>();

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

    /// <summary>
    /// Publica um evento de integração (IntegrationEvent) para todos os handlers registrados.
    /// </summary>
    public async Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken ct = default)
    {
        if (integrationEvent == null)
            throw new ArgumentNullException(nameof(integrationEvent));

        var eventType = integrationEvent.GetType();
        var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

        using var scope = _provider.CreateScope();
        var handlers = scope.ServiceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var method = handlerType.GetMethod("HandleAsync")!;
            if (method == null)
                throw new InvalidOperationException($"O handler {handlerType.Name} não possui método HandleAsync.");

            var task = (Task)method.Invoke(handler, new object[] { integrationEvent, ct })!;
            await task;
        }
    }
}
