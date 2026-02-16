using GBastos.Casa_dos_Farelos.Domain.Abstractions;
using GBastos.Casa_dos_Farelos.Domain.Events.Compras;
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;
using GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;

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
                i.NomeProduto,
                i.Quantidade,
                i.CustoUnitario,
                i.SubTotal))
            .ToList();

        return new CompraCriadaIntegrationEvent(
            e.CompraId,
            e.FornecedorId,
            e.ValorTotal,
            itens
        );
    }
}