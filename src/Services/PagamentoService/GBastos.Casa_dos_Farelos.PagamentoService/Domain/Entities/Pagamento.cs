using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Common;
using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Enums;
using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Events;

namespace GBastos.Casa_dos_Farelos.PagamentoService.Domain.Entities;

public sealed class Pagamento : AggregateRoot
{
    public Guid PedidoId { get; private set; }
    public Guid ClienteId { get; private set; }

    public string Moeda { get; private set; } = null!;
    public string MetodoPagamento { get; private set; } = null!;
    public string IdempotencyKey { get; private set; } = null!;

    public decimal ValorPG { get; private set; }

    public DateTime CriadoEmUtc { get; private set; }
    public DateTime? ProcessadoEmUtc { get; private set; }

    public StatusPagamento Status { get; private set; }

    public Pedido Pedido { get; private set; } = null!;
    public Cliente Cliente { get; private set; } = null!;

    private Pagamento() { }

    public static Pagamento CriarPagamento(
        Guid pedidoId,
        Guid clienteId,
        decimal valor,
        string metodoPagamento,
        string moeda)
    {
        if (pedidoId == Guid.Empty)
            throw new ArgumentException("PedidoId inválido.");

        if (clienteId == Guid.Empty)
            throw new ArgumentException("ClienteId inválido.");

        if (valor <= 0)
            throw new ArgumentException("Valor deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(metodoPagamento))
            throw new ArgumentException("Método de pagamento inválido.");

        if (string.IsNullOrWhiteSpace(moeda))
            throw new ArgumentException("Moeda inválida.");

        var pagamento = new Pagamento
        {
            Id = Guid.NewGuid(),
            PedidoId = pedidoId,
            ClienteId = clienteId,
            ValorPG = valor,
            MetodoPagamento = metodoPagamento,
            Moeda = moeda,
            Status = StatusPagamento.Pendente,
            CriadoEmUtc = DateTime.UtcNow
        };

        pagamento.AddDomainEvent(
            new PagamentoCriadoEvent(
                pagamento.Id,
                pagamento.PedidoId,
                pagamento.ValorPG));

        return pagamento;
    }

    public void Confirmar()
    {
        if (Status != StatusPagamento.Pendente)
            throw new InvalidOperationException("Pagamento já processado.");

        Status = StatusPagamento.Aprovado;
        ProcessadoEmUtc = DateTime.UtcNow;

        AddDomainEvent(
            new PagamentoAprovadoEvent(Id, PedidoId, ClienteId, ValorPG));
    }

    public void Recusar(string motivo)
    {
        if (Status != StatusPagamento.Pendente)
            throw new InvalidOperationException("Pagamento já processado.");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("Motivo é obrigatório.");

        Status = StatusPagamento.Recusado;
        ProcessadoEmUtc = DateTime.UtcNow;

        AddDomainEvent(
            new PagamentoRecusadoEvent(Id, PedidoId, motivo));
    }

    public void AddIdempotencyKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new Exception("IdempotencyKey obrigatória");

        IdempotencyKey = key;
    }

    internal static Pagamento CriarPedido(
        Guid pedidoId,
        decimal valor,
        Guid clienteId)
    {
        return CriarPagamento(
            pedidoId,
            clienteId,
            valor,
            metodoPagamento: "PIX",
            moeda: "BRL");
    }

    internal void PublishDomainEvents()
    {
        throw new NotImplementedException();
    }
}