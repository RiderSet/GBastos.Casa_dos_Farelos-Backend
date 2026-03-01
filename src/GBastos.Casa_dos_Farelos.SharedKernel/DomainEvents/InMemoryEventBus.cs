using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;
using Microsoft.Extensions.DependencyInjection;

namespace GBastos.Casa_dos_Farelos.SharedKernel.DomainEvents;

public sealed class InMemoryEventBus : IEventBus
{
    private readonly IServiceScopeFactory _scopeFactory;

    public InMemoryEventBus(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task Publish<T>(T @event, CancellationToken ct = default)
        where T : class
    {
        using var scope = _scopeFactory.CreateScope();

        var handlers = scope.ServiceProvider
            .GetServices<IEventHandler<T>>();

        await Task.WhenAll(
            handlers.Select(h => h.Handle(@event, ct))
        );
    }

    public async Task Publish(object @event, CancellationToken ct = default)
    {
        if (@event is null)
            throw new ArgumentNullException(nameof(@event));

        using var scope = _scopeFactory.CreateScope();

        var eventType = @event.GetType();
        var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

        var handlers = scope.ServiceProvider.GetServices(handlerType);

        var tasks = handlers.Select(handler =>
        {
            var method = handler!.GetType().GetMethod("Handle")
                ?? throw new InvalidOperationException("Handler inválido.");

            return (Task)method.Invoke(handler, new object[] { @event, ct })!;
        });

        await Task.WhenAll(tasks);
    }

    public async Task PublishAsync(
        IIntegrationEvent integrationEvent,
        CancellationToken ct = default)
    {
        if (integrationEvent is null)
            throw new ArgumentNullException(nameof(integrationEvent));

        using var scope = _scopeFactory.CreateScope();

        var eventType = integrationEvent.GetType();
        var handlerType =
            typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

        var handlers = scope.ServiceProvider.GetServices(handlerType);

        var tasks = handlers.Select(handler =>
        {
            var method = handler!.GetType().GetMethod("HandleAsync")
                ?? throw new InvalidOperationException("Handler inválido.");

            return (Task)method.Invoke(handler, new object[] { integrationEvent, ct })!;
        });

        await Task.WhenAll(tasks);
    }
}