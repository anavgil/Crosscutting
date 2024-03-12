using Carter;
using Crosscutting.Api.Middlewares;
using Crosscutting.Common.Configurations.Global;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

namespace Crosscutting.Api.DependencyInjection;
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCrosscuttingApi(this IApplicationBuilder app)
    {
        GlobalSettings settings = new();

        app.UseCrosscuttingApi(settings);

        return app;
    }

    public static IApplicationBuilder UseCrosscuttingApi(this IApplicationBuilder app, Action<GlobalSettings> setupSettings = null)
    {
        GlobalSettings settings = new();

        setupSettings?.Invoke(settings);

        app.UseCrosscuttingApi(settings);

        return app;
    }

    public static IApplicationBuilder UseCrosscuttingApi(this IApplicationBuilder app, GlobalSettings settings)
    {
        app.UseSerilogRequestLogging();
        app.UseRequestSecurity();
        app.UseExceptionHandler();

        if (settings.UseRateLimit)
            app.UseRateLimiter();

        if (settings.UseCarter)
            app.UseCarterMiddleware();

        if (settings.UseHealthChecks)
            app.UseHealthchecksMiddleware();

        return app;
    }

    private static IApplicationBuilder UseCarterMiddleware(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapCarter();
        });

        return app;
    }

    private static IApplicationBuilder UseHealthchecksMiddleware(this IApplicationBuilder app)
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

        return app;
    }
}
