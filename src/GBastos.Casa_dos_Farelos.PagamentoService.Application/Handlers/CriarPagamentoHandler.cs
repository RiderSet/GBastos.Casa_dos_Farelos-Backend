using GBastos.Casa_dos_Farelos.PagamentoService.Application.Commands;
using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Aggregates;
using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Handlers;

public class CriarPagamentoHandler
    : IRequestHandler<CriarPagamentoCommand, Guid>
{
    private readonly IPagamentoRepository _repository;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<CriarPagamentoHandler> _logger;

    public CriarPagamentoHandler(
        IPagamentoRepository repository,
        IUnitOfWork uow,
        ILogger<CriarPagamentoHandler> logger)
    {
        _repository = repository;
        _uow = uow;
        _logger = logger;
    }

    public async Task<Guid> Handle(
        CriarPagamentoCommand request,
        CancellationToken cancellationToken)
    {
        using var transaction =
            await _uow.BeginTransactionAsync(cancellationToken);

        try
        {
            var pagamento = Pagamento.CriarPagamento(
                request.PedidoId,
                request.ClienteId,
                request.Valor,
                request.MetodoPagamento,
                request.Moeda);

            pagamento.AddIdempotencyKey(request.IdempotencyKey);

            await _repository.AddAsync(pagamento, cancellationToken);
            await _uow.CommitAsync(cancellationToken);

            pagamento.PublishDomainEvents();

            await transaction.CommitAsync(cancellationToken);

            return pagamento.Id;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}