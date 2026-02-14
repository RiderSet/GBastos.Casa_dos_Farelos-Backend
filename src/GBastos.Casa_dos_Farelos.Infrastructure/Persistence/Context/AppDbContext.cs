using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Seed.General;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<ClientePF> ClientesPF => Set<ClientePF>();
    public DbSet<ClientePJ> ClientesPJ => Set<ClientePJ>();
    public DbSet<Fornecedor> Fornecedores => Set<Fornecedor>();
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    public DbSet<Venda> Vendas => Set<Venda>();
    public DbSet<ItemVenda> ItensVenda => Set<ItemVenda>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();
    public DbSet<Compra> Compras => Set<Compra>();
    public DbSet<ItemCompra> ItensCompra => Set<ItemCompra>();
    public DbSet<Carrinho> Carrinhos => Set<Carrinho>();
    public DbSet<DataSeedHistory> DataSeedHistory => Set<DataSeedHistory>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    DbSet<ItemCompra> IAppDbContext.ItensCompra => throw new NotImplementedException();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await base.SaveChangesAsync(ct);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---------------- HERANÇA PESSOA ----------------
        modelBuilder.Entity<Pessoa>()
            .ToTable("Pessoas")                 // Toda a hierarquia em uma tabela
            .HasDiscriminator<string>("Tipo")   // Coluna discriminadora
            .HasValue<ClientePF>("PF")
            .HasValue<ClientePJ>("PJ");

        // Configure propriedades específicas
        modelBuilder.Entity<ClientePF>().Property(c => c.CPF).HasMaxLength(11);
        modelBuilder.Entity<ClientePJ>().Property(c => c.CNPJ).HasMaxLength(14);

        // ---------------- PRODUTO ----------------
        modelBuilder.Entity<Produto>().ToTable("Produtos");
        modelBuilder.Entity<Produto>().Property(p => p.Nome).IsRequired();
        modelBuilder.Entity<Produto>().Property(p => p.DescricaoProduto).IsRequired();
        modelBuilder.Entity<Produto>().Property(p => p.PrecoVenda).HasPrecision(18, 2);

        // ---------------- COMPRA ----------------
        modelBuilder.Entity<Compra>().ToTable("Compras");
        modelBuilder.Entity<Compra>().Property(c => c.DataCompra).IsRequired();
     //   modelBuilder.Entity<Compra>().Property(c => c.TotalCompra).HasPrecision(18, 2);

        // BACKING FIELD para Itens
        modelBuilder.Entity<Compra>()
            .Metadata.FindNavigation(nameof(Compra.Itens))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

      //  modelBuilder.Entity<CriarCompraCommand>().ToTable("ItensCompra");

        // ---------------- VENDA ----------------
        modelBuilder.Entity<Venda>().ToTable("Vendas");
        modelBuilder.Entity<Venda>()
            .Metadata.FindNavigation(nameof(Venda.Itens))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        modelBuilder.Entity<ItemVenda>().ToTable("ItensVenda");

        // ---------------- RELACIONAMENTO PRODUTO-CATEGORIA ----------------
        modelBuilder.Entity<Produto>()
            .HasOne(p => p.Categoria)
            .WithMany(c => c.Produtos)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DataSeedHistory>()
            .ToTable("__DataSeedHistory")
            .HasKey(x => x.Id);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}