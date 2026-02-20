using FluentValidation;
using GBastos.Casa_dos_Farelos.Api.Extensions;
using GBastos.Casa_dos_Farelos.Application.Abstraction;
using GBastos.Casa_dos_Farelos.Application.Common;
using GBastos.Casa_dos_Farelos.Application.Interfaces;
using GBastos.Casa_dos_Farelos.Application.Validators.Behaviors;
using GBastos.Casa_dos_Farelos.Domain.Common;
using GBastos.Casa_dos_Farelos.Infrastructure.DependencyInjection;
<<<<<<< HEAD
using GBastos.Casa_dos_Farelos.Infrastructure.Messaging;
=======
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
>>>>>>> 532a5516c5422679921d3b0f6d7a9995a5d30bda
using GBastos.Casa_dos_Farelos.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.DataMigrations;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Interceptors;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Seed.General;
using GBastos.Casa_dos_Farelos.Infrastructure.Repositories;
using GBastos.Casa_dos_Farelos.Infrastructure.Services;
using GBastos.Casa_dos_Farelos.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ======== Configuração ========
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// ======== Scan de IDataMigration ========
builder.Services.Scan(scan => scan
    .FromAssemblies(
    typeof(IIntegrationEvent).Assembly,
    typeof(IEventHandler<>).Assembly,
    typeof(AppDbContext).Assembly
).AddClasses(c => c.AssignableTo<IDataMigration>()).AsImplementedInterfaces().WithScopedLifetime());

// ======== Validators & MediatR ========
builder.Services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyMarker>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
});

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// ======== Interceptors ========
builder.Services.AddScoped<DomainValidationInterceptor>();
builder.Services.AddScoped<PublishDomainEventsInterceptor>();
builder.Services.AddScoped<OutboxSaveChangesInterceptor>();

<<<<<<< HEAD
builder.Services.AddSingleton(async sp =>
{
    return await RabbitMqConnection.CreateAsync(
        builder.Configuration["Rabbit:Host"]!,
        builder.Configuration["Rabbit:User"]!,
        builder.Configuration["Rabbit:Pass"]!
    );
});

//Console.WriteLine("CONNECTION STRING = " + builder.Configuration.GetConnectionString("Conn"));
=======
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();

>>>>>>> 532a5516c5422679921d3b0f6d7a9995a5d30bda
// ======== Database ========
builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    var validation = sp.GetRequiredService<DomainValidationInterceptor>();
    var publish = sp.GetRequiredService<PublishDomainEventsInterceptor>();
    var outbox = sp.GetRequiredService<OutboxSaveChangesInterceptor>();

    var connectionString = config.GetConnectionString("SqlServer")!;

    options.UseSqlServer(connectionString, sql =>
        sql.EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null));

    // ORDEM CORRETA (IMPORTANTÍSSIMO)
    options.AddInterceptors(
        validation,
        publish,
        outbox
    );
});

// ======== Swagger ========
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Casa dos Farelos API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Informe o token JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };

    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, Array.Empty<string>() } });
});

// ======== JWT ========
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key não configurado"));

builder.Services.AddInfrastructure(builder.Configuration);

// builder.Services.AddTransient<IIntegrationEventHandler<ClienteCriadoIntegrationEvent>, ClienteCriadoHandler>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Vendedor", p => p.RequireClaim("role", "Vendedor", "Gerente"));
    options.AddPolicy("Gerente", policy => policy.RequireRole("Gerente"));
});

// ======== Build App ========
var app = builder.Build();

// ======== Middleware ========
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Casa dos Farelos API v1"));
}

app.UseExceptionHandler(handler =>
{
    handler.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception is DomainException domain)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { erro = domain.Message });
            return;
        }

        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new { erro = "Erro interno no servidor" });
    });
});

app.UseAuthentication();
app.UseAuthorization();

// ======== Endpoints ========
app.MapEndpoints();

// ======== Database Seed & Migrations ========
await using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();

    await db.Database.MigrateAsync();                   // aplica migrations
    await UserSeed.SeedAsync(db);                       // seeds de usuários
    await DataMigrationRunner.RunAsync(services, CancellationToken.None);
    await DatabaseSeeder.RunAsync(services, CancellationToken.None);
}

// ======== Run App ========
app.Run();