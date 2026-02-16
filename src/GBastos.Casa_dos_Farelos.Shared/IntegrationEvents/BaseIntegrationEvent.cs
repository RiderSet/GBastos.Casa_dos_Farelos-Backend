using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Domain.Abstractions;

public abstract record BaseIntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Momento em que o fato ocorreu no domínio
    /// </summary>
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Momento em que foi persistido no Outbox
    /// </summary>
    public DateTime CreatedOn { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Nome do evento para desserialização
    /// </summary>
    public string EventType => GetType().Name;

    /// <summary>
    /// Rastreia toda a requisição (HTTP -> fila -> outro serviço)
    /// </summary>
    public Guid? CorrelationId { get; init; }

    /// <summary>
    /// Evento que originou este evento
    /// </summary>
    public Guid? CausationId { get; init; }

    /// <summary>
    /// Versão do contrato de integração
    /// </summary>
    public int Version { get; init; } = 1;

    /// <summary>
    /// Id da entidade raiz
    /// </summary>
    public Guid? AggregateId { get; init; }

    /// <summary>
    /// Usuário responsável
    /// </summary>
    public Guid? UserId { get; init; }

    /// <summary>
    /// Multi-tenant
    /// </summary>
    public Guid? TenantId { get; init; }
}