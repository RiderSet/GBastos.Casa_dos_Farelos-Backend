namespace GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;

public interface IOutboxDispatcher
{
    Task DispatchOutboxAsync(CancellationToken ct = default);
}