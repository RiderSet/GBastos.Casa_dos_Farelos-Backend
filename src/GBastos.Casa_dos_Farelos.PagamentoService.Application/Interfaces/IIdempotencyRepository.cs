namespace GBastos.Casa_dos_Farelos.PagamentoService.Application.Interfaces;

public interface IIdempotencyRepository
{
    Task<bool> ExistsAsync(string key, CancellationToken ct);
    Task AddAsync(string key, CancellationToken ct);
}