using GBastos.Casa_dos_Farelos.PedidoService.Domain.Domain.Events;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Entities;
using GBastos.Casa_dos_Farelos.PedidoService.Domain.Enum;
using GBastos.Casa_dos_Farelos.SharedKernel.Exceptions;

namespace GBastos.Casa_dos_Farelos.PedidoService.Domain.Aggregates;

public class Pedido : AggregateRoot<Guid>
{
    private readonly List<ItemPedido> _itens = new();

    public Guid ClienteId { get; private set; }
    public StatusPedido Status { get; private set; }
    public decimal Total { get; private set; }

    public IReadOnlyCollection<ItemPedido> Itens => _itens;

    protected Pedido() : base(Guid.Empty) { }

    public Pedido(Guid clienteId)
        : base(Guid.NewGuid())
    {
        if (clienteId == Guid.Empty)
            throw new DomainException("Cliente inválido.");

        ClienteId = clienteId;
        Status = StatusPedido.Criado;
    }

    public void AdicionarItem(
        Guid produtoId,
        string nomeProduto,
        int quantidade,
        decimal precoUnitario)
    {
        if (Status != StatusPedido.Criado)
            throw new DomainException("Não é possível alterar o pedido.");

        if (produtoId == Guid.Empty)
            throw new DomainException("Produto inválido.");

        if (string.IsNullOrWhiteSpace(nomeProduto))
            throw new DomainException("Nome do produto obrigatório.");

        if (quantidade <= 0)
            throw new DomainException("Quantidade inválida.");

        if (precoUnitario <= 0)
            throw new DomainException("Preço inválido.");

        var item = new ItemPedido(
            Id,
            produtoId,
            nomeProduto,
            quantidade,
            precoUnitario);

        _itens.Add(item);

        RecalcularTotal();
    }

    public void Confirmar()
    {
        if (!_itens.Any())
            throw new DomainException("Pedido sem itens.");

        if (Status != StatusPedido.Criado)
            throw new DomainException("Pedido já processado.");

        Status = StatusPedido.Confirmado;

        AddDomainEvent(
            new PedidoConfirmadoEvent(
                Id,
                ClienteId,
                Total
            ));
    }

    public void Cancelar()
    {
        if (Status == StatusPedido.Pago)
            throw new DomainException("Não pode cancelar pedido pago.");

        Status = StatusPedido.Cancelado;

        AddDomainEvent(
            new PedidoCanceladoEvent(
                Id,
                ClienteId
            ));
    }

    private void RecalcularTotal()
    {
        Total = _itens.Sum(i => i.SubTotal);
    }

    public static Pedido Criar(object clienteId)
    {
        throw new NotImplementedException();
    }
}