namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;

public interface IIdempotencyRepository
{
    Task<bool> ExistsAsync(string key, CancellationToken ct);
    Task AddAsync(string key, CancellationToken ct);
}