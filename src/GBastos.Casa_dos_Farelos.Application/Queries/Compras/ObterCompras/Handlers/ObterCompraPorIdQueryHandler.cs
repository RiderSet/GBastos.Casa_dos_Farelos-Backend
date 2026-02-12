using GBastos.Casa_dos_Farelos.Application.Dtos;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Application.Queries.Compras.ObterCompras;
using MediatR;

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

        var itensDto = compra.Itens.Select(i => new ItemCompraDto
        {
            ProdutoId = i.ProdutoId,
            DescricaoProduto = "", // opcional: você pode buscar a descrição do Produto
            Quantidade = i.Quantidade,
            PrecoUnitario = i.CustoUnitario,
            SubTotal = i.SubTotal
        }).ToList();

        return new CompraDto
        {
            Id = compra.Id,
            FornecedorId = compra.FornecedorId,
            DataCompra = compra.DataCompra,
            TotalCompra = compra.TotalCompra,
            Itens = itensDto
        };
    }
}