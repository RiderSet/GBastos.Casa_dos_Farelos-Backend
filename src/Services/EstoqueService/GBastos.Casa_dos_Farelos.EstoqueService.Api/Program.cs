using FluentValidation;
using GBastos.Casa_dos_Farelos.EstoqueService.Api.Endpoints;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Behavior;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.DependencyInjections;
using GBastos.Casa_dos_Farelos.EstoqueService.Application.Interfaces;
using GBastos.Casa_dos_Farelos.EstoqueService.Infrastructure.Outbox;
using MassTransit;
using MediatR;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddHostedService<OutboxWorker>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(Program).Assembly);

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddSingleton<IEventTypeResolver, EventTypeResolver>();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis")
        ?? throw new InvalidOperationException("Redis não configurado.");

    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddSingleton<IDistributedLockFactory>(sp =>
{
    var multiplexer = sp.GetRequiredService<IConnectionMultiplexer>();

    return RedLockFactory.Create(new List<RedLockMultiplexer>
    {
        new RedLockMultiplexer(multiplexer)
    });
});

var app = builder.Build();

EstoqueEndpoints.Map(app);

app.Run();