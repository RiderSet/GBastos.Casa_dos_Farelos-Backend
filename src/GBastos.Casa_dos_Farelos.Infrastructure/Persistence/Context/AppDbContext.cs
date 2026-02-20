using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Domain.Entities;
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Seed.General;
using Microsoft.EntityFrameworkCore;

namespace GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;

public sealed class AppDbContext : DbContext,
    IAppDbContext,
    IOutboxDbContext,
    ISeedHistoryDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // ================= DbSets =================

    public DbSet<Pessoa> Pessoas => Set<Pessoa>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Fornecedor> Fornecedores => Set<Fornecedor>();
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();
    public DbSet<Compra> Compras => Set<Compra>();
    public DbSet<ItemCompra> ItensCompra => Set<ItemCompra>();
    public DbSet<ItemVenda> ItensVenda => Set<ItemVenda>();
    public DbSet<Venda> Vendas => Set<Venda>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Carrinho> Carrinhos => Set<Carrinho>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<DataSeedHistory> DataSeedHistory => Set<DataSeedHistory>();

    IQueryable<Carrinho> IAppDbContext.Carrinhos => Carrinhos.AsQueryable();
    IQueryable<Venda> IAppDbContext.Vendas => Vendas.AsQueryable();
    IQueryable<Produto> IAppDbContext.Produtos => Produtos.AsQueryable();

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        => base.SaveChangesAsync(ct);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) return;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ===================== PESSOA / CLIENTE (TPH) =====================
        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.ToTable("Pessoas");
            entity.HasKey(p => p.Id);

            entity.HasDiscriminator<string>("Tipo")
                  .HasValue<ClientePF>("PF")
                  .HasValue<ClientePJ>("PJ");

            entity.Property(p => p.Nome).IsRequired();
            entity.Property(p => p.Email).IsRequired();
            entity.Property(p => p.Telefone).IsRequired();
            entity.Property(p => p.DtCadastro).IsRequired();
        });

        modelBuilder.Entity<ClientePF>(entity =>
        {
            entity.Property(c => c.CPF).HasMaxLength(11).IsRequired();
            entity.Property(c => c.DtNascimento).IsRequired();
        });

        modelBuilder.Entity<ClientePJ>(entity =>
        {
            entity.Property(c => c.CNPJ).HasMaxLength(14).IsRequired();
            entity.Property(c => c.RazaoSocial).IsRequired();
            entity.Property(c => c.NomeFantasia).IsRequired();
            entity.Property(c => c.Contato).IsRequired(false);
        });

        // ===================== PRODUTO =====================
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.ToTable("Produtos");
            entity.Property(p => p.Nome).IsRequired();
            entity.Property(p => p.DescricaoProduto).IsRequired();
            entity.Property(p => p.PrecoVenda).HasPrecision(18, 2);

            entity.HasOne(p => p.Categoria)
                  .WithMany(c => c.Produtos)
                  .HasForeignKey(p => p.CategoriaId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ===================== VENDA =====================
        modelBuilder.Entity<Compra>(entity =>
        {
            entity.ToTable("Compras");

            entity.HasMany(c => c.Itens)
                  .WithOne(i => i.Compra)
                  .HasForeignKey(i => i.CompraId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.Carrinho)
                  .WithMany()
                  .HasForeignKey(c => c.Id)
                  .OnDelete(DeleteBehavior.SetNull); // se carrinho for deletado, compra permanece
        });

        modelBuilder.Entity<ItemVenda>()
            .ToTable("ItensVenda");

        modelBuilder.Entity<ItemPedido>()
            .ToTable("ItensPedido");

        // ===================== CARRINHO =====================
        modelBuilder.Entity<Carrinho>(entity =>
        {
            entity.ToTable("Carrinhos");

            entity.HasMany(c => c.Itens)
                  .WithOne()
                  .HasForeignKey("CarrinhoId")
                  .OnDelete(DeleteBehavior.Cascade);

            //entity.Navigation(nameof(Carrinho.Itens))
            //      .HasField("_itens")
            //      .UsePropertyAccessMode(PropertyAccessMode.Field);

            //entity.OwnsMany(typeof(CarrinhoItem), "_itens", item =>
            //{
            //    item.ToTable("CarrinhoItens");

            //    item.WithOwner()
            //        .HasForeignKey("CarrinhoId");

            //    item.Property<Guid>("Id");
            //    item.HasKey("Id");

            //    item.Property<Guid>("ProdutoId").IsRequired();
            //    item.Property<int>("Quantidade").IsRequired();
            //    item.Property<decimal>("PrecoUnitario").HasPrecision(18, 2);
            //});
        });

        // ===================== OUTBOX =====================
        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("OutboxMessages");
            entity.Property(o => o.EventName).IsRequired();
            entity.Property(o => o.OccurredOnUtc).IsRequired();
        });

        // ===================== DATA SEED HISTORY =====================
        modelBuilder.Entity<DataSeedHistory>(entity =>
        {
            entity.ToTable("__DataSeedHistory");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.AppliedOnUtc).IsRequired();
        });

        // 🔥 Aplicar configurações UMA ÚNICA VEZ
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}