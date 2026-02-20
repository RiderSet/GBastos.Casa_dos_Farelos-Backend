using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Events.Compras;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.EventHandlers.Compras;

public sealed class CompraCriadaEstoqueHandler
    : INotificationHandler<CompraCriadaDomainEvent>
{
    private readonly IProdutoRepository _produtoRepository;

    public CompraCriadaEstoqueHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task Handle(CompraCriadaDomainEvent notification, CancellationToken ct)
    {
        foreach (var item in notification.Itens)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(item.ProdutoId, ct);

            if (produto is null)
                continue;

            produto.EntradaEstoque(item.Quantidade);
        }

     // await _produtoRepository.UnitOfWork.SaveChangesAsync(ct);
    }
}