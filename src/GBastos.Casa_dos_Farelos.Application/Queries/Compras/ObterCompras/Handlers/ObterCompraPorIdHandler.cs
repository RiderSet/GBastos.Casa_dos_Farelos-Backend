using GBastos.Casa_dos_Farelos.Application.Dtos;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Application.Queries.Compras.ObterCompras.Handlers;

public sealed class ObterCompraPorIdHandler
    : IRequestHandler<ObterCompraPorIdQuery, CompraDetalhadaDto?>
{
    private readonly IAppDbContext _db;

    public ObterCompraPorIdHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<CompraDetalhadaDto?> Handle(
        ObterCompraPorIdQuery request,
        CancellationToken cancellationToken)
    {
        var compra = await _db.Compras
            .Include(x => x.Itens)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (compra is null)
            return null;

        return new CompraDetalhadaDto
        {
            Id = compra.Id,
            DataCompra = compra.DataCompra,
            FornecedorId = compra.FornecedorId,
            Total = compra.TotalCompra, 
            Itens = compra.Itens.Select(i => new ItemCompraDto
            {
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.CustoUnitario,
                SubTotal = i.SubTotal
            }).ToList()
        };
    }
}
