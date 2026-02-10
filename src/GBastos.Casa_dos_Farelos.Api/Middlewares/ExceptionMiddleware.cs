using GBastos.Casa_dos_Farelos.Domain.Common;
using System.Net;
using System.Text.Json;

namespace GBastos.Casa_dos_Farelos.Api.Middlewares;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, ex.Message);

        var response = context.Response;
        response.ContentType = "application/json";

        var errors = new List<string>();
        var statusCode = HttpStatusCode.InternalServerError;

        switch (ex)
        {
            // FluentValidation
            case FluentValidation.ValidationException validationEx:
                statusCode = HttpStatusCode.BadRequest;
                errors = validationEx.Errors
                    .Select(e => e.ErrorMessage)
                    .Distinct()
                    .ToList();
                break;

            // Regras de domínio
            case DomainException domainEx:
                statusCode = HttpStatusCode.UnprocessableEntity;
                errors.Add(domainEx.Message);
                break;

            // Não encontrado
            case KeyNotFoundException notFoundEx:
                statusCode = HttpStatusCode.NotFound;
                errors.Add(notFoundEx.Message);
                break;

            // Não autorizado
            case UnauthorizedAccessException unauthorizedEx:
                statusCode = HttpStatusCode.Unauthorized;
                errors.Add(unauthorizedEx.Message);
                break;

            default:
                errors.Add("Ocorreu um erro interno no servidor");
                break;
        }

        response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(new { errors });

        await response.WriteAsync(json);
    }
}
