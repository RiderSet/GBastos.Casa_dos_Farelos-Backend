using FluentValidation;
using GBastos.Casa_dos_Farelos.Api.Extensions;
using GBastos.Casa_dos_Farelos.Application.Common;
using GBastos.Casa_dos_Farelos.Application.Validators.Behaviors;
using GBastos.Casa_dos_Farelos.Infrastructure.DependencyInjection;
using GBastos.Casa_dos_Farelos.Infrastructure.Extensioons;
using GBastos.Casa_dos_Farelos.Infrastructure.Interfaces;
using GBastos.Casa_dos_Farelos.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Migrations;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Seed.General;
using GBastos.Casa_dos_Farelos.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.Scan(scan => scan
    .FromAssemblies(Assembly.GetExecutingAssembly())
    .AddClasses(c => c.AssignableTo<IDataMigration>())
    .AsImplementedInterfaces()
    .WithScopedLifetime());

// ======== Validators & MediatR ========
builder.Services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyMarker>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
});
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddScoped<OutboxSaveChangesInterceptor>();

// ======== Database ========
builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
   //var env = sp.GetRequiredService<IHostEnvironment>();
    var env = builder.Environment;
    var redisPassword = SecretProvider.TryGet("redis_password");
    var config = sp.GetRequiredService<IConfiguration>();

    string connectionString;

    if (env.IsDevelopment())
    {
        connectionString = builder.Configuration.GetConnectionString("Conn")!;
    }
    else
    {
        var dbPassword = SecretProvider.GetRequired("sql_password");

        connectionString =
            $"Server=sqlserver,1433;" +
            $"Database=CasaDosFarelos;" +
            $"User Id=sa;" +
            $"Password={dbPassword};" +
            $"TrustServerCertificate=True";
    }

    // Redis, JWT e RabbitMQ
    var redisConnection = redisPassword is null
        ? "redis:6379"
        : $"redis:6379,password={redisPassword}";

    var jwtKey = env.IsDevelopment()
        ? builder.Configuration["Jwt:Key"]!
        : SecretProvider.GetRequired("jwt_secret");

    var rabbitUser = SecretProvider.TryGet("rabbit_user") ?? "guest";
    var rabbitPass = SecretProvider.TryGet("rabbit_password") ?? "guest";

    // registra no configuration runtime
    builder.Configuration["ConnectionStrings:Conn"] = connectionString;
    builder.Configuration["Redis:Connection"] = redisConnection;
    builder.Configuration["Jwt:Key"] = jwtKey;
    builder.Configuration["Rabbit:User"] = rabbitUser;
    builder.Configuration["Rabbit:Password"] = rabbitPass;

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

var keyValue = jwt["Key"]
    ?? throw new InvalidOperationException("Jwt:Key não configurado");

var key = Encoding.UTF8.GetBytes(keyValue);

builder.Services.AddInfrastructure(builder.Configuration);

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

using (var scope = app.Services.CreateScope())
{
    await DistributedDbInitializer.EnsureMigratedAsync(scope.ServiceProvider, CancellationToken.None);
}

app.UseAuthentication();
app.UseAuthorization();

// ======== Endpoints ========
app.MapEndpoints();

// ======== Database Seed ========
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var retry = 0;
    while (retry < 10)
    {
        try
        {
            await db.Database.MigrateAsync();
            break;
        }
        catch
        {
            retry++;
            await Task.Delay(5000);
        }
    }

    await UserSeed.SeedAsync(db);
}

await using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;

    await DataMigrationRunner.RunAsync(services, CancellationToken.None); // EF migrations
    await DatabaseSeeder.RunAsync(services, CancellationToken.None);          // seeds base
    await DataMigrationRunner.RunAsync(services, CancellationToken.None);     // ⭐ novo sistema
}

using (var scope = app.Services.CreateScope())
{
    await DataMigrationRunner.RunAsync(scope.ServiceProvider, CancellationToken.None);
}

await EnsureDatabaseAsync(app);

app.Run();

// --============================================================
static async Task EnsureDatabaseAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILoggerFactory>()
                         .CreateLogger("DatabaseStartup");

    try
    {
        var db = services.GetRequiredService<AppDbContext>();

        logger.LogInformation("Aplicando migrations...");
        await db.Database.MigrateAsync();

        logger.LogInformation("Executando seeds...");
        await DatabaseSeeder.RunAsync(services, CancellationToken.None);

        logger.LogInformation("Banco pronto 🚀");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro ao preparar banco");
        throw;
    }
}