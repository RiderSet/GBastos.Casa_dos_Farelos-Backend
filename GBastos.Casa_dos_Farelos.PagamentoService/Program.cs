using GBastos.Casa_dos_Farelos.PagamentoService.Domain.Interfaces;
using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.PagamentoService.Infrastructure.Persistence.UOF;
using GBastos.Casa_dos_Farelos.PagamentoService.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddScoped<IIntegrationEventOutbox, IntegrationEventOutbox>();
builder.Services.AddScoped<IOutboxDispatcher, OutboxDispatcher>();
builder.Services.AddHostedService<OutboxBackgroundService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

app.Run();
