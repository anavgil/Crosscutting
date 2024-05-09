using Crosscutting.Api.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Sinks.OpenTelemetry;

namespace Crosscutting.Api.DependencyInjection;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseCrosscuttingLogging(this IHostBuilder builder)
    {
        return builder.UseSerilog((context, logger) =>
        {
            logger
                .Enrich.FromLogContext()
                .Enrich.WithSpan()
                .ReadFrom.Configuration(context.Configuration)
                ;

            //if (context.HostingEnvironment.IsDevelopment())
            //{
            //    logger.WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} {TraceId} {Level:u3} {Message}{NewLine}{Exception}");
            //}
            //else
            //{
            //    logger.WriteTo.Console(new JsonFormatter());
            //}
        });
    }
    public static IHostBuilder UseCrosscuttingLoggingWithOPT(this IHostBuilder builder,IConfiguration configuration)
    {
        ObservabilityOptions observabilityOptions = new();

        configuration
            .GetRequiredSection(nameof(ObservabilityOptions))
            .Bind(observabilityOptions);



        return builder.UseSerilog((context, provider, options) =>
        {
            var environment = context.HostingEnvironment.EnvironmentName;
            var configuration = context.Configuration;

            ObservabilityOptions observabilityOptions = new();

            configuration
                .GetSection(nameof(ObservabilityOptions))
                .Bind(observabilityOptions);

            var serilogSection = $"{nameof(ObservabilityOptions)}:{nameof(ObservabilityOptions)}:Serilog";

            options
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                //.Enrich.WithEnvironment(environment)
                .Enrich.WithProperty("ApplicationName", observabilityOptions.ServiceName);

            /* ============== */
            /* Only export to OpenTelemetry collector */
            /* ============== */
            options.WriteTo.OpenTelemetry(cfg =>
            {
                cfg.Endpoint = $"{observabilityOptions.CollectorUrl}/v1/logs";
                cfg.IncludedData = IncludedData.TraceIdField | IncludedData.SpanIdField;
                cfg.ResourceAttributes = new Dictionary<string, object>
                                                {
                                                    {"service.name", observabilityOptions.ServiceName},
                                                    {"index", 10},
                                                    {"flag", true},
                                                    {"value", 3.14}
                                                };
            });
        }
}
