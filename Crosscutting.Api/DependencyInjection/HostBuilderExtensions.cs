using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;

namespace Crosscutting.Api.DependencyInjection;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseLogging(this IHostBuilder builder)
    {
        return builder.UseSerilog((context, logger) =>
        {
            logger
                .Enrich.FromLogContext()
                .Enrich.WithSpan();

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
