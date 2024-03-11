using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;

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
}
