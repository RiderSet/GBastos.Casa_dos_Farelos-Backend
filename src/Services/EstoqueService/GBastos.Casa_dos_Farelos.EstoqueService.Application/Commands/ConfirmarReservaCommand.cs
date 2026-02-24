using MediatR;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Commands;

public sealed record ConfirmarReservaCommand(
    Guid ReservaId,
    string IdempotencyKey)
    : IRequest<bool>;