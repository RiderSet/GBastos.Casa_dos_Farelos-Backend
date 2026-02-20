using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Dtos;
using GBastos.Casa_dos_Farelos.Domain.Entities;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly ClientePFRepository _pfRepo;
    private readonly ClientePJRepository _pjRepo;

    public ClienteRepository(ClientePFRepository pfRepo, ClientePJRepository pjRepo)
    {
        _pfRepo = pfRepo;
        _pjRepo = pjRepo;
    }

    public Task AddAsync(Pessoa cliente, CancellationToken ct = default)
    {
        return cliente switch
        {
            ClientePF pf => _pfRepo.AddAsync(pf, ct),
            ClientePJ pj => _pjRepo.AddAsync(pj, ct),
            _ => throw new ArgumentException("Tipo de cliente inválido")
        };
    }

    public Task<List<ClienteListDto>> ListarAsync(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async Task<Pessoa?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
    {
        var clientePF = await _pfRepo.ObterPorIdAsync(id, ct);
        if (clientePF != null)
            return clientePF;

        var clientePJ = await _pjRepo.ObterPorIdAsync(id, ct);
        return clientePJ;
    }
}