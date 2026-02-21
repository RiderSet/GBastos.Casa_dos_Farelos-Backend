using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Application.Queries.Compras.ObterCompras;
using GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;
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

        // Mapeia itens
        var itensDto = compra.Itens
            .Select(i => new ItemCompraDto(
                i.ProdutoId,
                i.NomeProduto,
                i.Quantidade,
                i.CustoUnitario
            ))
            .ToList();

        // Calcula valor total
        var valorTotal = itensDto.Sum(x => x.SubTotal);

        // Retorna DTO da compra
        return new CompraDto
        {
            Id = compra.Id,
            FornecedorId = compra.FornecedorId,
            FuncionarioId = compra.FuncionarioId,
            DataCompra = compra.DataCompra,
            Finalizada = compra.Finalizada,
            Itens = itensDto
        };
    }
}