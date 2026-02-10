namespace GBastos.Casa_dos_Farelos.Shared.Interfaces;

public interface IEventHandler<TEvent>
{
    Task Handle(TEvent @event, CancellationToken ct);
}
