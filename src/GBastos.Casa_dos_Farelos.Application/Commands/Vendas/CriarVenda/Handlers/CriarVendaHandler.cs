using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Vendas.CriarVenda.Handlers;

public sealed class CriarVendaHandler : IRequestHandler<CriarVendaCommand, Guid>
{
    private readonly IProdutoRepository _produtoRepo;
    private readonly IVendaSaveRepository _vendaRepo;

    public CriarVendaHandler(
        IProdutoRepository produtoRepo,
        IVendaSaveRepository vendaRepo)
    {
        _produtoRepo = produtoRepo;
        _vendaRepo = vendaRepo;
    }

    public async Task<Guid> Handle(CriarVendaCommand request, CancellationToken ct)
    {
        if (request.Itens.Count == 0)
            throw new DomainException("Venda precisa possuir itens");

        var produtos = await _produtoRepo.ObterPorIdsAsync(
            request.Itens.Select(i => i.ProdutoId), ct);

        var itens = new List<ItemVenda>();

        foreach (var item in request.Itens)
        {
            if (!produtos.TryGetValue(item.ProdutoId, out var produto))
                throw new DomainException($"Produto {item.ProdutoId} não encontrado");

            produto.BaixarEstoque(item.Quantidade);

            itens.Add(new ItemVenda(
                produto.Id,
                item.Quantidade,
                produto.PrecoVenda
            ));
        }

        var venda = Venda.Criar(request.ClienteId, request.FuncionarioId, itens);

        await _vendaRepo.AddAsync(venda, ct);
        await _vendaRepo.SaveChangesAsync(ct);

        return venda.Id;
    }
}