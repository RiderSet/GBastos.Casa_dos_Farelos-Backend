using MediatR;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Application.Commands;

public record CriarPagamentoCommand(
    Guid PedidoId,
    Guid ClienteId,
    decimal Valor,
    string MetodoPagamento,
    string Moeda,
    string IdempotencyKey
) : IRequest<Guid>;