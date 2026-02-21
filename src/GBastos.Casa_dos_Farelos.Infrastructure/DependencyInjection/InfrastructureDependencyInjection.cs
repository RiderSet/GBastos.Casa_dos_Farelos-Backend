using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Application.Security;
using GBastos.Casa_dos_Farelos.Infrastructure.Caching;
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Interceptors;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.ReadModels.Relatorios;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.UnitOfWorks;
using GBastos.Casa_dos_Farelos.Infrastructure.Repositories;
using GBastos.Casa_dos_Farelos.Shared.Events.Compras;
using GBastos.Casa_dos_Farelos.Shared.IntegrationEvents;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace GBastos.Casa_dos_Farelos.Infrastructure.DependencyInjection;

public static class InfrastructureDependencyInjection
{
    public static object AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ------------------ DATABASE (EF Core) ------------------
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Conn")));

        // ------------------ DAPPER ------------------
        services.AddScoped<IDbConnection>(_ =>
            new SqlConnection(configuration.GetConnectionString("Conn")));

        // ------------------ REPOSITORIES / SERVICES ------------------
        services.AddScoped<JwtService>();
        services.AddScoped<IVendaSaveRepository, VendaSaveRepository>();
        services.AddScoped<IVendaReadRepository, VendaReadRepository>();
        services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<ICompraRepository, CompraRepository>();
        services.AddScoped<IIntegrationEventMapper, IntegrationEventMapping>();
        services.AddScoped<IOutboxDispatcher, OutboxDispatcher>();
        services.AddScoped<IIntegrationEventOutbox, OutboxService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<PublishDomainEventsInterceptor>();

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IOutboxDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<ISeedHistoryDbContext>(sp => sp.GetRequiredService<AppDbContext>());

        services.AddScoped<IClientePFRepository, ClientePFRepository>();
        services.AddScoped<IClientePJRepository, ClientePJRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<ClientePFRepository>();
        services.AddScoped<ClientePJRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();

        services.AddScoped<IVendaRepository, VendaRepository>();

        // ------------------ EVENT BUS ------------------
        services.AddSingleton<IEventBus, InMemoryEventBus>();
        services.AddSingleton<IIntegrationEventTypeResolver, IntegrationEventTypeResolver>();

        // ------------------ HOSTED SERVICES ------------------
        services.AddHostedService<OutboxProcessor>();
        services.AddHostedService<OutboxWorker>();

        // ------------------ AUTOMATIC HANDLERS ------------------
        services.Scan(scan => scan
            .FromApplicationDependencies(a => a.FullName!.Contains("Casa_dos_Farelos"))

            .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()

            .AddClasses(c => c.AssignableTo(typeof(IIntegrationEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()

            .AddClasses(c => c.AssignableTo<IDataMigration>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        // ------------------ QUERY SERVICES (RELATÓRIOS) ------------------
        services.AddScoped<IRelatorioVendasQueryService, RelatorioVendasQueryService>();

        // ------------------ CACHE ------------------
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetSection("Redis:Connection").Value;
            options.InstanceName = "CasaDosFarelos:";
        });

        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}