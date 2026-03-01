namespace GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

public interface IEventHandler<TEvent>
{
    Task Handle(TEvent @event, CancellationToken ct);
}
