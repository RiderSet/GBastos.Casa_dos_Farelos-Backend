using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Compras.ObterCompras.Handlers;

public sealed class ObterComprasHandler
    : IRequestHandler<ObterComprasQuery, List<CompraResumoDto>>
{
    private readonly IAppDbContext _db;

    public ObterComprasHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CompraResumoDto>> Handle(
        ObterComprasQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.Compras
            .AsNoTracking()
            .AsQueryable();

        if (request.DataInicio.HasValue)
            query = query.Where(x => x.DataCompra >= request.DataInicio.Value);

        if (request.DataFim.HasValue)
            query = query.Where(x => x.DataCompra <= request.DataFim.Value);

        return await query
            .OrderByDescending(x => x.DataCompra)
            .Select(x => new CompraResumoDto
            {
                Id = x.Id,
                DataCompra = x.DataCompra,
                Total = x.ValorTotal, // ← domínio calcula
                FornecedorId = x.FuncionarioId
            })
            .ToListAsync(cancellationToken);
    }
}
