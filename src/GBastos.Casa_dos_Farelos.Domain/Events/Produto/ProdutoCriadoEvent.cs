using GBastos.Casa_dos_Farelos.Domain.Abstractions;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Produto
{
    public sealed class ProdutoCriadoEvent : IDomainEvent
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

        public Guid ProdutoId { get; init; }
        public string Nome { get; init; } = default!;
    }
}
