namespace GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;

public sealed record EstoqueConfirmadoEvent(
    Guid ReservaId,
    DateTime ConfirmadoEm);