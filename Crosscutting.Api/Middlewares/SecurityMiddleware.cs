using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Crosscutting.Api.Middlewares;

public class SecurityMiddleware
{
    private readonly RequestDelegate _next;
    public SecurityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        //Avoid Click Jacking
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        //Avoid MIME Sniffing
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        //Avoid Cross Site Scripting(XSS)
        context.Response.Headers.Append("X-Xss-Protection", "1; mode=block");
        context.Response.Headers.Append("Referrer-Policy", "no-referrer");
        //context.Response.Headers.Add("Content-Security-Policy", "default-src 'self';");
        //context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self'; style-src 'self'; font-src 'self'; img-src 'self'; frame-src 'self'");




        await _next(context);
    }
}

public static class SecurityMiddlewareExtension
{
    public static IApplicationBuilder UseRequestSecurity(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecurityMiddleware>();
    }
}
