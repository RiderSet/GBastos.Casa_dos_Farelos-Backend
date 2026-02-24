namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public interface IOutboxRepository
{
    Task AddAsync<T>(T @event, CancellationToken ct);
}