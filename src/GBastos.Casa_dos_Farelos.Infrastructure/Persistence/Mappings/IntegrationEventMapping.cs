using GBastos.Casa_dos_Farelos.Domain.Events.Compras;
using GBastos.Casa_dos_Farelos.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.Shared.Dtos.Compras;
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
            .Select(i => new ItemCompraDto(
                i.ProdutoId,
                i.NomeProduto,
                i.Quantidade,
                i.CustoUnitario))
            .ToList();

        return new CompraCriadaIntegrationEvent(
            e.CompraId,
            e.FuncionarioId,
            e.ValorTotal,
            itens
        );
    }
}