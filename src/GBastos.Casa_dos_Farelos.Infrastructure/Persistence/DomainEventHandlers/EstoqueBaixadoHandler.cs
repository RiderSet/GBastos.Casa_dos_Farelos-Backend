using GBastos.Casa_dos_Farelos.Domain.Estoque;
using GBastos.Casa_dos_Farelos.Domain.Events;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using MediatR;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.DomainEventHandlers;

public sealed class EstoqueBaixadoHandler
    : INotificationHandler<EstoqueBaixadoDomainEvent>
{
    private readonly AppDbContext _db;

    public EstoqueBaixadoHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task Handle(EstoqueBaixadoDomainEvent notification, CancellationToken ct)
    {
        var movimentacao = new MovimentacaoEstoque(
            notification.ProdutoId,
            notification.NomeProduto,
            notification.Quantidade,
            "SAIDA"
        );

        await _db.Set<MovimentacaoEstoque>().AddAsync(movimentacao, ct);
    }
}