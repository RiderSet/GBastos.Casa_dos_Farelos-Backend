using GBastos.Casa_dos_Farelos.Application.Dtos;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Application.Queries.Compras.ObterCompras;
using MediatR;
using System.Linq;

public sealed class ObterCompraPorIdQueryHandler
    : IRequestHandler<ObterCompraPorIdQuery, CompraDto?>
{
    private readonly ICompraRepository _repo;

    public ObterCompraPorIdQueryHandler(ICompraRepository repo)
    {
        _repo = repo;
    }

    public async Task<CompraDto?> Handle(ObterCompraPorIdQuery request, CancellationToken ct)
    {
        var compra = await _repo.ObterPorIdAsync(request.Id, ct);
        if (compra is null) return null;

        var itensDto = compra.Itens
            .Select(i => new CompraItemDto(
                i.ProdutoId,
                i.NomeProduto,
                i.Quantidade,
                i.CustoUnitario,
                i.SubTotal
            ))
            .ToList();

        return new CompraDto(
            compra.Id,
            compra.FornecedorId,
            compra.TotalCompra,
            itensDto
        );
    }
}