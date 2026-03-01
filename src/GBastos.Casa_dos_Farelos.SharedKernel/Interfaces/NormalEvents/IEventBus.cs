using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

public interface IEventBus
{
    Task Publish<T>(T @event, CancellationToken ct = default) where T : class;
    Task Publish(object @event, CancellationToken ct = default);
    Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken ct);
}
