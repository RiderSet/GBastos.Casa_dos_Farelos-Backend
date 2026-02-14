using GBastos.Casa_dos_Farelos.Application.Commands.Compras.IntegrationsEvents;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Events.Compras;
using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;

public sealed class IntegrationEventMapping : IIntegrationEventMapper
{
    public IIntegrationEvent Map(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            CompraCriadaDomainEvent e => MapCompraCriada(e),
            _ => throw new InvalidOperationException($"No mapper for {domainEvent.GetType().Name}")
        };
    }

    private static IIntegrationEvent MapCompraCriada(CompraCriadaDomainEvent e)
    {
        var itens = e.Itens
            .Select(i => new CompraItemDto(
                i.ProdutoId,
                i.Quantidade,
                i.PrecoUnitario))
            .ToList();

        return new CompraCriadaIntegrationEvent(
            e.CompraId,
            e.FornecedorId,
            e.Total,
            itens
        );
    }
}