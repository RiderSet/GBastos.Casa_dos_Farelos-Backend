using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Compras.CriarCompra.Handlers;

public sealed class CriarCompraHandler : IRequestHandler<CriarCompraCommand, Guid>
{
    private readonly IAppDbContext _db;

    public CriarCompraHandler(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CriarCompraCommand request, CancellationToken cancellationToken)
    {
        var fornecedorExiste = await _db.Fornecedores
            .AnyAsync(f => f.Id == request.FornecedorId, cancellationToken);

        if (!fornecedorExiste)
            throw new DomainException("Fornecedor não encontrado.");

        var compra = Compra.Criar(request.FornecedorId, request.FuncionarioId);

        foreach (var item in request.Itens)
        {
            var produto = await _db.Produtos
                .FirstOrDefaultAsync(p => p.Id == item.ProdutoId, cancellationToken)
                ?? throw new DomainException($"Produto {item.ProdutoId} não encontrado.");

            compra.AdicionarProduto(
                item.ProdutoId,
                item.NomeProduto,
                item.Quantidade,
                item.CustoUnitario
            );

            produto.EntradaEstoque(item.Quantidade);
        }

        compra.Finalizar();

        _db.Compras.Add(compra);

        await _db.SaveChangesAsync(cancellationToken);

        return compra.Id;
    }
}