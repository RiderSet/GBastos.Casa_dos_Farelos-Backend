using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Estoques.Handlers;

public sealed class BaixarEstoqueCommandHandler
    : IRequestHandler<BaixarEstoqueCommand, Unit>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BaixarEstoqueCommandHandler(
        IProdutoRepository produtoRepository,
        IUnitOfWork unitOfWork)
    {
        _produtoRepository = produtoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(BaixarEstoqueCommand request, CancellationToken ct)
    {
        var produto = await _produtoRepository
            .ObterPorIdAsync(request.ProdutoId, ct);

        if (produto is null)
            throw new DomainException("Produto não encontrado.");

        produto.BaixarEstoque(request.Quantidade);

        try
        {
            await _unitOfWork.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new DomainException(
                "O produto foi atualizado por outro processo. Tente novamente.");
        }

        return Unit.Value;
    }
}