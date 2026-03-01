using GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Interfaces;
using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Handlers;

public sealed class ConfirmarReservaCommandHandler
    : IRequestHandler<ConfirmarReservaCommand, bool>
{
    private readonly IEstoqueUnitOfWork _uow;
    private readonly IDistributedLockFactory _lockFactory;

    public ConfirmarReservaCommandHandler(
        IEstoqueUnitOfWork uow,
        IDistributedLockFactory lockFactory)
    {
        _uow = uow;
        _lockFactory = lockFactory;
    }

    public async Task<bool> Handle(
        ConfirmarReservaCommand request,
        CancellationToken cancellationToken)
    {
        if (await _uow.Idempotency.ExistsAsync(
            request.IdempotencyKey, cancellationToken))
            return true;

        var resource = $"reserva:{request.ReservaId}";

        await using var redLock = await _lockFactory.CreateLockAsync(
            resource,
            TimeSpan.FromSeconds(30),
            TimeSpan.FromSeconds(10),
            TimeSpan.FromMilliseconds(200));

        if (!redLock.IsAcquired)
            throw new Exception("Lock não adquirido.");

        await _uow.BeginTransactionAsync(cancellationToken);

        var reserva = await _uow.Reservas
            .GetByIdAsync(request.ReservaId, cancellationToken);

        if (reserva is null)
            throw new Exception("Reserva não encontrada.");

        if (reserva.Confirmada)
            return true;

        reserva.Confirmar();

        await _uow.Idempotency.AddAsync(
            request.IdempotencyKey, cancellationToken);

        var evento = new EstoqueConfirmadoEvent(reserva.Id, DateTime.UtcNow);

        await _uow.Outbox.AddAsync(evento, cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);
        await _uow.CommitAsync(cancellationToken);

        return true;
    }
}