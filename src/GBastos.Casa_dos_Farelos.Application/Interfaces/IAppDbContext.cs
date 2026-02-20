using GBastos.Casa_dos_Farelos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Pessoa> Pessoas { get; }
    DbSet<Usuario> Usuarios { get; }
  //  DbSet<Produto> Produtos { get; }
    DbSet<Fornecedor> Fornecedores { get; }
    DbSet<Funcionario> Funcionarios { get; }
   // DbSet<Venda> Vendas { get; }
    DbSet<ItemVenda> ItensVenda { get; }
    DbSet<ItemPedido> ItensPedido { get; }
    DbSet<Compra> Compras { get; }
    DbSet<ItemCompra> ItensCompra { get; }
  //  DbSet<Carrinho> Carrinhos { get; }

    IQueryable<Carrinho> Carrinhos { get; }
    IQueryable<Venda> Vendas { get; }
    IQueryable<Produto> Produtos { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
