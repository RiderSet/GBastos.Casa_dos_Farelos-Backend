using GBastos.Casa_dos_Farelos.CadastroService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.CadastroService.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly CadastroDbContext _context;

    public ClienteRepository(CadastroDbContext context)
    {
        _context = context;
    }

    public async Task<Cliente?> ObterPorIdAsync(Guid id)
        => await _context.Clientes.FindAsync(id);

    public async Task AdicionarAsync(Cliente cliente)
        => await _context.Clientes.AddAsync(cliente);

    public async Task<bool> EmailJaExisteAsync(string email)
        => await _context.Clientes
            .AnyAsync(x => x.Email == email);
}