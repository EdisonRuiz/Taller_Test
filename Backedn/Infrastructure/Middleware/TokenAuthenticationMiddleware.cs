using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http;

namespace Infrastructure.Middleware;

public class TokenAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private const string TOKEN = "my-demo-token"; // Hardcoded demo token

    public TokenAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication for Swagger UI and OpenAPI endpoints
        if (context.Request.Path.StartsWithSegments("/swagger") ||
            context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("Authorization", out var tokenHeader))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Missing Authorization header");
            return;
        }

        var token = tokenHeader.ToString().Replace("Bearer ", "");

        if (!string.Equals(token, TOKEN, StringComparison.Ordinal))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Invalid or expired token");
            return;
        }

        await _next(context);
    }
}
