using FluentValidation;
using GBastos.Casa_dos_Farelos.Api.Extensions;
using GBastos.Casa_dos_Farelos.Application.Common;
using GBastos.Casa_dos_Farelos.Application.Validators.Behaviors;
using GBastos.Casa_dos_Farelos.Infrastructure.DependencyInjection;
using GBastos.Casa_dos_Farelos.Infrastructure.Outbox;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Context;
using GBastos.Casa_dos_Farelos.Infrastructure.Persistence.Seed;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

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
    var interceptor = sp.GetRequiredService<OutboxSaveChangesInterceptor>();

    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Conn"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)
    );

    options.AddInterceptors(interceptor);
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

app.Run();