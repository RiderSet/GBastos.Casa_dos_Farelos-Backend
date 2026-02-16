using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Dtos;
using GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Compras.ObterCompras.Handlers;

public sealed class ObterCompraPorIdHandler
    : IRequestHandler<ObterCompraPorIdQuery, CompraDto?>
{
    private readonly IAppDbContext _db;

    public ObterCompraPorIdHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<CompraDto?> Handle(
        ObterCompraPorIdQuery request,
        CancellationToken cancellationToken)
    {
        var compra = await _db.Compras
            .Include(x => x.Itens)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (compra is null)
            return null;

        return new CompraDto(
            compra.Id,
            compra.FornecedorId,
            compra.ValorTotal,
            compra.Itens.Select(i => new CompraItemDto(
                i.ProdutoId,
                i.NomeProduto,
                i.Quantidade,
                i.CustoUnitario,
                i.SubTotal
            )).ToList()
        );
    }
}