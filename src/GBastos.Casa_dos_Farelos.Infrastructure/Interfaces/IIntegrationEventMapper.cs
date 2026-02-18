using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;

public interface IIntegrationEventMapper
{
    IIntegrationEvent Map(IDomainEvent domainEvent);
}