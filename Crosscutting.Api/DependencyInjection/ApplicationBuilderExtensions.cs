using Carter;
using Crosscutting.Api.Middlewares;
using Crosscutting.Common.Configurations.Global;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

namespace Crosscutting.Api.DependencyInjection;
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCrosscuttingBase(this IApplicationBuilder app)
    {
        GlobalSettings settings = new();

        app.UseCrosscuttingBase(settings);

        return app;
    }

    public static IApplicationBuilder UseCrosscuttingBase(this IApplicationBuilder app, GlobalSettings settings)
    {
        app.UseRequestSecurity();
        app.UseExceptionHandler();

        if (settings.UseRateLimit)
            app.UseRateLimiter();

        app.UseSerilogRequestLogging();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapCarter();
        });

        if (settings.UseHealthChecks)
        {
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
        }

        return app;
    }
}
