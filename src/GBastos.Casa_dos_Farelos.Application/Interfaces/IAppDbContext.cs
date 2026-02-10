using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Produto> Produtos { get; }
        DbSet<Fornecedor> Fornecedores { get; }
        DbSet<Compra> Compras { get; }
        DbSet<ItemCompra> ItensCompra { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
