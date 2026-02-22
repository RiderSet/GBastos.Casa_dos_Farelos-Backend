namespace GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;

public sealed record PagamentoCriadoIntegrationEvent
{
    /// <summary>
    /// Id único da mensagem (idempotência distribuída)
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Versão do contrato para evolução futura
    /// </summary>
    public int Version { get; init; } = 1;

    /// <summary>
    /// Data de criação do evento
    /// </summary>
    public DateTime OccurredOn { get; init; }

    /// <summary>
    /// Identificador do Pedido
    /// </summary>
    public Guid PedidoId { get; init; }

    /// <summary>
    /// Identificador do Cliente
    /// </summary>
    public Guid ClienteId { get; init; }

    /// <summary>
    /// Valor total do pagamento
    /// </summary>
    public decimal Valor { get; init; }

    /// <summary>
    /// Moeda (ex: BRL, USD)
    /// </summary>
    public string Moeda { get; init; } = default!;

    /// <summary>
    /// Forma de pagamento (PIX, CartaoCredito, Boleto...)
    /// </summary>
    public string MetodoPagamento { get; init; } = default!;

    /// <summary>
    /// Correlação distribuída (trace)
    /// </summary>
    public string? CorrelationId { get; init; }

    /// <summary>
    /// Serviço que originou o evento
    /// </summary>
    public string OriginService { get; init; } = default!;
}