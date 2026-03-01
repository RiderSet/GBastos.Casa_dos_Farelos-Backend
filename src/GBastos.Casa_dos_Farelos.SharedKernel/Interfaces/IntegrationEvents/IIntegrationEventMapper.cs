using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Application.Interfaces;

public interface IIntegrationEventMapper
{
    IIntegrationEvent? Map(IDomainEvent domainEvent);
}