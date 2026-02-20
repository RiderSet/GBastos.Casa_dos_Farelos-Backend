using MediatR;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Estoques;

public sealed record BaixarEstoqueCommand(
    Guid ProdutoId,
    int Quantidade,
    byte[] RowVersion
) : IRequest<Unit>;