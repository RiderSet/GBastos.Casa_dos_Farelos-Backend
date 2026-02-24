using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;

public sealed record ReservarEstoqueCommand(
    Guid ProdutoId,
    Guid PedidoId,
    int Quantidade,
    string IdempotencyKey,
    string CorrelationId
) : IRequest<bool>;