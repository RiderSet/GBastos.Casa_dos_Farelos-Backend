namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public interface IOutboxDispatcher
{
    Task DispatchAsync(CancellationToken cancellationToken = default);
}
