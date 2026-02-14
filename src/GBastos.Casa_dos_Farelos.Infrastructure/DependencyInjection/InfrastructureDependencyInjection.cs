using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Application.Security;
using GBastos.Casa_dos_Farelos.Domain.Abstractions;
using GBastos.Casa_dos_Farelos.Infrastructure.Caching;
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Mappings;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.ReadModels.Relatorios;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.UnitOfWorks;
using GBastos.Casa_dos_Farelos.Infrastructure.Repositories;
using GBastos.Casa_dos_Farelos.Shared.Events;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace GBastos.Casa_dos_Farelos.Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ------------------ DATABASE (EF) ------------------
            //services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(configuration.GetConnectionString("Conn")));

            // ------------------ DAPPER ------------------
            services.AddScoped<IDbConnection>(_ =>
                new SqlConnection(configuration.GetConnectionString("Conn")));

            // ------------------ REPOSITORIES ------------------

            services.AddScoped<JwtService>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<IClientePFRepository, ClientePFRepository>();
            services.AddScoped<IClientePJRepository, ClientePJRepository>();
            services.AddScoped<IVendaSaveRepository, VendaSaveRepository>();
            services.AddScoped<IVendaReadRepository, VendaReadRepository>();
            services.AddScoped<IIntegrationEvent, IntegrationEvent>();
            services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<ICompraRepository, CompraRepository>();
            services.AddScoped<IIntegrationEventMapper, IntegrationEventMapping>();
            services.AddScoped<IOutbox, OutboxService>();
            services.AddHostedService<OutboxProcessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IEventBus, InMemoryEventBus>();
            services.AddHostedService<OutboxProcessor>();

            services.Scan(scan => scan
                .FromApplicationDependencies()
                .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            services.Scan(scan => scan
                .FromAssemblyOf<IDataMigration>()
                .AddClasses(c => c.AssignableTo<IDataMigration>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            // ------------------ QUERY SERVICES (RELATORIOS) ------------------
            services.AddScoped<IRelatorioVendasQueryService, RelatorioVendasQueryService>();

            // ------------------ CACHE ------------------
            //services.AddMemoryCache();
            //services.AddScoped<ICacheService, MemoryCacheService>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetSection("Redis:Connection").Value;
                options.InstanceName = "CasaDosFarelos:";
            });

            services.AddScoped<ICacheService, RedisCacheService>();

            return services;
        }
    }
}
