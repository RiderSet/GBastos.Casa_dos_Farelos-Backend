namespace GBastos.Casa_dos_Farelos.Domain.Interfaces;

public interface IOutboxDispatcher
{
    Task DispatchOutboxAsync(CancellationToken ct = default);
}