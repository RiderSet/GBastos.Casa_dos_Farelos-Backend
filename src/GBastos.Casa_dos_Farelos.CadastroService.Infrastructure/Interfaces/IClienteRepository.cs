using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;

public interface IClienteRepository
{
    Task<Cliente?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Cliente cliente);
    Task<bool> EmailJaExisteAsync(string email);
}