using MediatR;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Application.Commands;

public record CriarPagamentoCommand(Guid PedidoId, decimal Valor) : IRequest<Guid>;