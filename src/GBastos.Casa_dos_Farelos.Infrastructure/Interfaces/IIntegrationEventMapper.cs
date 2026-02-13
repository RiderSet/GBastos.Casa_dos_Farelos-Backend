using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Interfaces;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;

public interface IIntegrationEventMapper
{
    IIntegrationEvent Map(IDomainEvent domainEvent);
}