using GBastos.Casa_dos_Farelos.EstoqueService.Application.IntegrationEvents;
using GBastos.Casa_dos_Farelos.EstoqueService.Domain.Events;
using GBastos.Casa_dos_Farelos.SharedKernel.Interfaces.NormalEvents;

namespace GBastos.Casa_dos_Farelos.EstoqueService.Application.Messaging;

public static class IntegrationEventRegistry
{
    public static IntegrationEvent? Map(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            ProdutoCriadoEvent e => new ProdutoCriadoIntegrationEvent
            {
                ProdutoId = e.ProdutoId,
                Nome = e.Nome,
                PrecoVenda = e.PrecoVenda
            },

            _ => null
        };
    }
}