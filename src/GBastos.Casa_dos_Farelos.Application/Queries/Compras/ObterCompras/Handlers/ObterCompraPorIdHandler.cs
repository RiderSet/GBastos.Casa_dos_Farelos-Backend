using GBastos.Casa_dos_Farelos.Application.Interfaces;
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

        // Mapeia itens para DTOs
        var itensDto = compra.Itens
            .Select(i => new ItemCompraDto
            {
                ProdutoId = i.ProdutoId,
                NomeProduto = i.NomeProduto,
                Quantidade = i.Quantidade,
                CustoUnitario = i.CustoUnitario,
                SubTotal = i.Quantidade * i.CustoUnitario
            })
            .ToList();

        // Calcula valor total da compra
        var valorTotal = itensDto.Sum(i => i.SubTotal);

        // Retorna DTO da compra
        return new CompraDto
        {
            Id = compra.Id,
            ClienteId = compra.ClienteId,
            FuncionarioId = compra.FuncionarioId,
            CarrinhoId = compra.CarrinhoId,
            DataCompra = compra.DataCompra,
            Finalizada = compra.Finalizada,
            Itens = itensDto
        };
    }
}