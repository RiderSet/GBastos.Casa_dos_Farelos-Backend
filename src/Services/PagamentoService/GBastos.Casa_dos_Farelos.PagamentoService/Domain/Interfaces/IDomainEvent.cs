namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Interfaces;

public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}