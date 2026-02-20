using GBastos.Casa_dos_Farelos.Domain.Common;

namespace GBastos.Casa_dos_Farelos.Domain.Events.Produto;

public sealed class ProdutoCriadoEvent : DomainEvent
{
<<<<<<< HEAD
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
=======
    public sealed class ProdutoCriadoEvent : IDomainEvent
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
>>>>>>> 532a5516c5422679921d3b0f6d7a9995a5d30bda

    public Guid ProdutoId { get; init; }
    public string ProdutoNome { get; init; }
    public decimal PrecoVenda { get; private set; }

    public ProdutoCriadoEvent(string produtonome, decimal precovenda)
    {
        ProdutoNome = produtonome;
        PrecoVenda = precovenda;
    }
}
