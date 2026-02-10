using GBastos.Casa_dos_Farelos.Application.Dtos;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Repositories
{
    public class VendaReadRepository : IVendaReadRepository
    {
        private readonly AppDbContext _db;

        public VendaReadRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<VendaDetalhadaDto?> ObterPorIdAsync(Guid id, CancellationToken ct)
        {
            var venda = await _db.Vendas
                .AsNoTracking()
                .Include(v => v.Itens)
                .FirstOrDefaultAsync(v => v.Id == id, ct);

            if (venda is null)
                return null;

            return new VendaDetalhadaDto
            {
                Id = venda.Id,
                DataVenda = venda.DataVenda,
                ClienteId = venda.ClienteId,
                FuncionarioId = venda.FuncionarioId,
                Total = venda.TotalVenda,

                Itens = venda.Itens.Select(i => new ItemVendaDto
                {
                    ProdutoId = i.ProdutoId,
                    Produto = i.DescricaoProduto,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    Total = i.SubTotal
                }).ToList()
            };
        }
    }
}
