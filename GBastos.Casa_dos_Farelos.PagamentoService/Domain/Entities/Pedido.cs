using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Common;
using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Enums;
using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events;
using PedidoCriadoDomainEvent = GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events.PedidoCriadoDomainEvent;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Entities;

public sealed class Pedido : BaseEntity
{
    public Guid ClienteId { get; private set; }
    public decimal Total { get; private set; }
    public PedidoStatus Status { get; private set; }
    public DateTime CriadoEmUtc { get; private set; }

    private Pedido() { }

    private Pedido(Guid clienteId, decimal total)
    {
        Id = Guid.NewGuid();
        ClienteId = clienteId;
        Total = total;
        Status = PedidoStatus.Criado;
        CriadoEmUtc = DateTime.UtcNow;

        AddDomainEvent(new PedidoCriadoDomainEvent(
            Id,
            ClienteId,
            Total,
            CriadoEmUtc));
    }

    public static Pedido Criar(Guid clienteId, decimal total)
    {
        if (total <= 0)
            throw new ArgumentException("Total inválido.");

        return new Pedido(clienteId, total);
    }

    public void MarcarComoPago()
    {
        if (Status != PedidoStatus.Criado)
            throw new InvalidOperationException("Pedido não pode ser pago.");

        Status = PedidoStatus.Pago;

        AddDomainEvent(new PedidoPagoDomainEvent(
            Id,
            ClienteId,
            Total,
            DateTime.UtcNow));
    }

    public void Cancelar()
    {
        if (Status == PedidoStatus.Pago)
            throw new InvalidOperationException("Pedido pago não pode ser cancelado.");

        Status = PedidoStatus.Cancelado;
    }
}