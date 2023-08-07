using Microsoft.AspNetCore.Builder;

namespace Crosscutting.Api;
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlerExtensions>();

        return app;
    }
}
