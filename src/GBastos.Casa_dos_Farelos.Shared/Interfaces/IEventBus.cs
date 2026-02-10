namespace GBastos.Casa_dos_Farelos.Shared.Interfaces
{
    public interface IEventBus
    {
        Task Publish<T>(T @event, CancellationToken ct = default) where T : class;
        Task Publish(object @event, CancellationToken ct = default);
    }
}
