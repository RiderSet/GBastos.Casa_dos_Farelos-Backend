using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Produtos.CriarProduto.Handlers;

public sealed class CriarProdutoHandler : IRequestHandler<CriarProdutoCommand, Guid>
{
    private readonly IProdutoRepository _repo;
    private readonly IUnitOfWork _uow;

    public CriarProdutoHandler(IProdutoRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<Guid> Handle(CriarProdutoCommand request, CancellationToken ct)
    {
        var produto = new Produto(
            request.Nome,
            request.Descricao,
            request.Preco,
            request.CategoriaId,
            request.QuantEstoque
        );

        await _repo.AddAsync(produto, ct);
        await _uow.CommitAsync(ct);

        return produto.Id;
    }
}
