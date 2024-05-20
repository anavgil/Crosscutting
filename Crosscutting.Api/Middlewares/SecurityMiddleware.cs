using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Crosscutting.Api.Middlewares;

public class SecurityMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    private readonly Dictionary<string, string> _cspPolicyCollection = new()
    {
        { "report","default-src 'self'; script-src 'self' 'unsafe-inline' scripts.ourdomain.com; style-src 'self' 'unsafe-inline' styles.ourdomain; img-src 'self' 'unsafe-inline' images.ourdomain.com; frame-ancestors 'none'" },
        { "default-lite","default-src 'self'" },
        { "default","default-src 'self'; script-src 'self'; style-src 'self'; font-src 'self'; img-src 'self'; frame-src 'self'" }
    };

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Remove("Server");
        context.Response.Headers.Remove("X-Powered-By");
        context.Response.Headers.Remove("X-AspNet-Version");
        context.Response.Headers.Remove("X-AspNetMvc-Version");
        //Avoid Click Jacking
        context.Response.Headers.Append("X-Frame-Options", new StringValues("DENY"));
        //Avoid MIME Sniffing
        context.Response.Headers.Append("X-Content-Type-Options", new StringValues("nosniff"));
        //Avoid Cross Site Scripting(XSS)
        context.Response.Headers.Append("X-Xss-Protection", "1; mode=block");
        context.Response.Headers.Append("Referrer-Policy", "no-referrer");
        context.Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store";

#if DEBUG
        context.Response.Headers.Append("Content-Security-Policy-Report-Only", new StringValues(_cspPolicyCollection["report"]));
#else
        context.Response.Headers.Append("Content-Security-Policy-Report-Only",new StringValues(_cspPolicyCollection["default"]));
#endif


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
