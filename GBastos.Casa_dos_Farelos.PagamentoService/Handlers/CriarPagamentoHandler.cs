using GBastos.Casa_dos_Farelos.PagamentoService.Application.Commands;
using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Entities;
using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Handlers;

public class CriarPagamentoHandler(
    IPagamentoRepository repository,
    IUnitOfWork uow)
    : IRequestHandler<CriarPagamentoCommand, Guid>
{
    private readonly IPagamentoRepository _repository = repository;
    private readonly IUnitOfWork _uow = uow;

    public async Task<Guid> Handle(CriarPagamentoCommand request, CancellationToken cancellationToken)
    {
        var pagamento = new Pagamento(request.PedidoId, request.Valor);

        await _repository.AddAsync(pagamento, cancellationToken);
        await _uow.CommitAsync(cancellationToken);

        return pagamento.Id;
    }
}