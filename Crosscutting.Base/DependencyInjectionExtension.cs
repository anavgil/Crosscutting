using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Formatting.Json;

namespace Crosscutting.Base
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection ConfigureBase(this IServiceCollection services)
        {

            services.AddSerilog();
            return services;
        }

        public static IHostBuilder UseLogging(this IHostBuilder builder)
        {
            return builder.UseSerilog((context, logger) =>
            {
                logger
                    .Enrich.FromLogContext()
                    .Enrich.WithSpan()
                    ;

                if (context.HostingEnvironment.IsDevelopment())
                {
                    logger.WriteTo.Console(
                        outputTemplate:
                        "{Timestamp:yyyy-MM-dd HH:mm:ss} {TraceId} {Level:u3} {Message}{NewLine}{Exception}");
                }
                else
                {
                    logger.WriteTo.Console(new JsonFormatter());
                }
            });
        }
    }
}