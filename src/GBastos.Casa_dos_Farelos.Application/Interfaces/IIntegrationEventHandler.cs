using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IIntegrationEventHandler<T>
    where T : IIntegrationEvent
{
    Task HandleAsync(T evt, CancellationToken ct);
}