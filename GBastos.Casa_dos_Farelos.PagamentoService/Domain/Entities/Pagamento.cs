using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Common;
using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Enums;
using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Entities;

public class Pagamento : AggregateRoot
{
 // public Guid Id { get; private set; }
    public Guid PedidoId { get; private set; }
    public decimal Valor { get; private set; }
    public StatusPagamento Status { get; private set; }

    private Pagamento() { }

    public Pagamento(Guid pedidoId, decimal valor)
    {
        Id = Guid.NewGuid();
        PedidoId = pedidoId;
        Valor = valor;
        Status = StatusPagamento.Pendente;

        AddDomainEvent(new PagamentoCriadoEvent(Id, PedidoId, Valor));
    }

    public void Confirmar()
    {
        Status = StatusPagamento.Aprovado;
        AddDomainEvent(new PagamentoAprovadoEvent(Id, PedidoId));
    }

    public void Recusar()
    {
        Status = StatusPagamento.Recusado;
        AddDomainEvent(new PagamentoRecusadoEvent(Id, PedidoId));
    }
}