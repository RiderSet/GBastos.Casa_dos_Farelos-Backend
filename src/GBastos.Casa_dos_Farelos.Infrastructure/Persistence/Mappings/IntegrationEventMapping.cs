using GBastos.Casa_dos_Farelos.Application.Commands.Compras.IntegrationsEvents;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Events.Compras;
using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Mappings;

public sealed class IntegrationEventMapper : IIntegrationEventMapper
{
    public IIntegrationEvent Map(IDomainEvent domainEvent)
    {
        if (domainEvent == null)
            throw new ArgumentNullException(nameof(domainEvent));

        return domainEvent switch
        {
            CompraCriadaDomainEvent e => new CompraCriadaIntegrationEvent(
                compraId: e.CompraId,
                fornecedorId: e.FornecedorId,
                valorTotal: e.ValorTotal,
                itens: e.Itens
            ),

            _ => throw new InvalidOperationException(
                $"Nenhum integration event mapeado para {domainEvent.GetType().Name}")
        };
    }
}