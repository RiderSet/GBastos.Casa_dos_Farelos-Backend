namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IOutboxDispatcher
{
    Task DispatchOutboxAsync(CancellationToken ct = default);
}