using Microsoft.AspNetCore.Mvc;

namespace GBastos.Casa_dos_Farelos.Api.Endpoints.Auth
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/auth").WithTags("Auth");

            group.MapPost("/login", Login)
                 .AllowAnonymous();

            return app;
        }

        private static IResult Login([FromBody] LoginRequest request)
        {
            // depois vamos integrar com banco + JWT Service
            if (request.User == "admin" && request.Password == "123")
                return Results.Ok(new { token = "TOKEN_AQUI" });

            return Results.Unauthorized();
        }

        public record LoginRequest(string User, string Password);
    }
}
