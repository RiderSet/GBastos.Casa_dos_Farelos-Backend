namespace GBastos.Casa_dos_Farelos.PagamentoService.Interfaces;

public interface IOutboxDispatcher
{
    Task DispatchAsync(CancellationToken cancellationToken);
}