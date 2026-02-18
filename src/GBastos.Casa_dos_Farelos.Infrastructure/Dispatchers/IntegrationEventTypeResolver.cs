using GBastos.Casa_dos_Farelos.Shared.Events.Clientes;
using GBastos.Casa_dos_Farelos.Shared.Events.Vendas;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Dispatchers;

public sealed class IntegrationEventTypeResolver : IIntegrationEventTypeResolver
{
    private readonly Dictionary<string, Type> _eventTypes = new()
        {
            { nameof(ClienteCriadoIntegrationEvent), typeof(ClienteCriadoIntegrationEvent) },
            { nameof(VendaCriadaIntegrationEvent), typeof(VendaCriadaIntegrationEvent) }
            // Adicione todos os eventos que sua aplicação produz
        };

    public Type Resolve(string eventName)
    {
        if (string.IsNullOrWhiteSpace(eventName))
            throw new ArgumentNullException(nameof(eventName));

        if (!_eventTypes.TryGetValue(eventName, out var type))
            throw new InvalidOperationException($"Tipo de evento '{eventName}' não registrado.");

        return type;
    }
}
