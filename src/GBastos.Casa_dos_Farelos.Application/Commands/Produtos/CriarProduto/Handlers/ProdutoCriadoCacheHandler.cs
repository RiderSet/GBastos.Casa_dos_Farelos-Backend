using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Events.Produto;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;

namespace GBastos.Casa_dos_Farelos.Application.Commands.Produtos.CriarProduto.Handlers
{
    public sealed class ProdutoCriadoCacheHandler : IEventHandler<ProdutoCriadoEvent>
    {
        private readonly ICacheService _cache;

        public ProdutoCriadoCacheHandler(ICacheService cache)
            => _cache = cache;

        public Task Handle(ProdutoCriadoEvent e, CancellationToken ct)
            => _cache.RemoveAsync($"produto:{e.ProdutoId}");
    }
}
