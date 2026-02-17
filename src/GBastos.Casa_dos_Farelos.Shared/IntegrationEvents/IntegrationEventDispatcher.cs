using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Events.Clientes;
using GBastos.Casa_dos_Farelos.Shared.Events.Vendas;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

public sealed class IntegrationEventDispatcher
{
    private readonly Dictionary<string, Func<string, IServiceProvider, CancellationToken, Task>> _routes;

    public IntegrationEventDispatcher()
    {
        _routes = new()
        {
            [typeof(ClienteCriadoEvent).Name] = Dispatch<ClienteCriadoEvent>,
            [typeof(VendaFinalizadaEvent).Name] = Dispatch<VendaFinalizadaEvent>
        };
    }

    private static async Task Dispatch<T>(
        string payload,
        IServiceProvider sp,
        CancellationToken ct)
        where T : class, IIntegrationEvent
    {
        var evt = JsonSerializer.Deserialize<T>(payload)!;
        var handlers = sp.GetServices<IIntegrationEventHandler<T>>();

        foreach (var handler in handlers)
            await handler.HandleAsync(evt, ct);
    }

    public Task DispatchAsync(string eventName, string payload, IServiceProvider sp, CancellationToken ct)
        => _routes[eventName](payload, sp, ct);
}