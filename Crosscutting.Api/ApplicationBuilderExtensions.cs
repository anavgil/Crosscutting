using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Crosscutting.Api;
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app)
    {
        app.UseRateLimiter();

        app.UseMiddleware<ExceptionHandlerExtensions>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapCarter();
        });

        var healthCheckOptions = new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status503ServiceUnavailable,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                }
        };
        app.UseHealthChecks("/health", healthCheckOptions);
        app.UseHealthChecksUI(config => { config.UIPath = "/health-ui"; });

        return app;
    }
}
