using Asp.Versioning;
using Crosscutting.Api.DependencyInjection;
using Crosscutting.Api.Endpoints;
using Crosscutting.Api.Middlewares;
using Crosscutting.Api.Options;
using Crosscutting.Common.Configurations.Global;
using HealthChecks.ApplicationStatus.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Reflection;
using System.Threading.RateLimiting;

namespace Crosscutting.Api.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCrosscuttingApi(this IServiceCollection services, IConfiguration configuration)
    {
        GlobalSettings settings = new();

        services.AddCrosscuttingApi(configuration, settings);

        return services;
    }

    public static IServiceCollection AddCrosscuttingApi(this IServiceCollection services, IConfiguration configuration, Action<GlobalSettings> setupSettings = null)
    {
        GlobalSettings settings = new();

        setupSettings?.Invoke(settings);

        services.AddCrosscuttingApi(configuration, settings);

        return services;
    }

    public static IServiceCollection AddCrosscuttingApi(this IServiceCollection services, IConfiguration configuration, GlobalSettings settings)
    {
        services.AddAntiforgery(options => { options.SuppressXFrameOptionsHeader = true; })
                //.AddExceptionHandler<GlobalExceptionHandler>()
                .AddExceptionHandler<CustomExceptionHandler>()
                .AddProblemDetails(options =>
                {
                    options.CustomizeProblemDetails = context =>
                    {
                        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                        Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);

                    };
                })
                .AddSerilog()
                .AddRouting()
                .AddCors();

        if (settings.UseRateLimit)
            services.AddRateLimiter(_ =>
            {
                _.OnRejected = (context, _) =>
                {
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        context.HttpContext.Response.Headers.RetryAfter =
                            ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                    }

                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", cancellationToken: _);

                    return new ValueTask();
                };

                _.AddFixedWindowLimiter(policyName: "fixed", options =>
                {
                    options.PermitLimit = 4;
                    options.Window = TimeSpan.FromSeconds(12);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = 2;
                });
            });

        if (settings.UseHealthChecks)
            services.AddHealthCheckDependencies();

        //if (settings.UseCarter)
        //    services.AddCarterDependencies();

        if (settings.UseOpenTelemetry)
            services.AddOpenTelemetryDependencies(configuration);

        if (settings.UseApiVersioning)
            services.AddApiVersioning();


        return services;
    }

    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        ServiceDescriptor[] serviceDescriptors = assembly
                                                .DefinedTypes
                                                .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                                                               type.IsAssignableTo(typeof(IEndpoint)))
                                                .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
                                                .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    #region Private methods
    private static IServiceCollection AddCors(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddDefaultPolicy(
            builder =>
            {
                builder.WithOrigins()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
            });
        });
    }

    private static IServiceCollection AddHealthCheckDependencies(this IServiceCollection services)
    {
        services.AddHealthChecks()
                .AddApplicationStatus(name: "api_status", tags: ["api"]);
        //.AddRedis("", name: "redis_status", tags: ["redis"]);
        services.AddHealthChecksUI()
                .AddInMemoryStorage();

        return services;
    }

    //private static IServiceCollection AddCarterDependencies(this IServiceCollection services)
    //{
    //    services.AddCarter(configurator: c =>
    //    {
    //        c.WithModules(GetCarterModules());
    //        //c.WithValidators(GetCarterValidation());
    //        c.WithEmptyValidators();
    //    });

    //    return services;
    //}

    //private static Type[] GetCarterModules()
    //{
    //    var assembly = Assembly.GetEntryAssembly();

    //    return assembly.GetTypes()
    //                .Where(t =>
    //                        !t.IsAbstract &&
    //                        typeof(ICarterModule).IsAssignableFrom(t) &&
    //                        t != typeof(ICarterModule) &&
    //                        t.IsPublic
    //                    ).ToArray();
    //}

    //private static Type[] GetCarterValidation()
    //{
    //    var assembly = Assembly.GetEntryAssembly();
    //    var applicationAssembly = assembly.GetReferencedAssemblies()
    //                                .Where(x => x.FullName.Contains("Application"))
    //                                .FirstOrDefault();

    //    if (applicationAssembly is null)
    //        return [];

    //    return Assembly.Load(applicationAssembly).GetTypes()
    //            .Where(t => !t.GetTypeInfo().IsAbstract && typeof(IValidator).IsAssignableFrom(t))
    //            .ToArray();
    //}

    private static IServiceCollection AddOpenTelemetryDependencies(this IServiceCollection services, IConfiguration configuration)
    {

        ObservabilityOptions observabilityOptions = new();

        configuration
            .GetRequiredSection(nameof(ObservabilityOptions))
            .Bind(observabilityOptions);

        services.AddOpenTelemetry()
                .AddTracing(observabilityOptions)
                .AddMetrics(observabilityOptions)
                ;

        return services;
    }

    private static IOpenTelemetryBuilder AddTracing(this IOpenTelemetryBuilder builder, ObservabilityOptions observabilityOptions)
    {
        builder.WithTracing(tracing =>
        {
            tracing
                     .AddSource(observabilityOptions.ServiceName)
                     .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(observabilityOptions.ServiceName))
                     .SetErrorStatusOnException()
                     .SetSampler(new AlwaysOnSampler())
                     .AddHttpClientInstrumentation()
                     .AddAspNetCoreInstrumentation(options =>
                     {
                         //options.EnableGrpcAspNetCoreSupport = true;
                         options.RecordException = true;
                     });

            /* Add more instrument here: MassTransit, NgSql ... */

            /* ============== */
            /* Only export to OpenTelemetry collector */
            /* ============== */
            tracing
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = observabilityOptions.CollectorUri;
                        options.ExportProcessorType = ExportProcessorType.Batch;
                        options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    });
        });

        return builder;
    }

    private static IOpenTelemetryBuilder AddMetrics(this IOpenTelemetryBuilder builder, ObservabilityOptions observabilityOptions)
    {
        builder.WithMetrics(metrics =>
        {
            var meter = new Meter(observabilityOptions.ServiceName);

            metrics
                     .AddMeter(meter.Name)
                     .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(meter.Name))
                     .AddAspNetCoreInstrumentation();

            /* Add more instrument here */

            /* ============== */
            /* Only export to OpenTelemetry collector */
            /* ============== */

            metrics
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = observabilityOptions.CollectorUri;
                        options.ExportProcessorType = ExportProcessorType.Batch;
                        options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    });
        });

        return builder;
    }

    private static IServiceCollection AddApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version"));
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
    #endregion
}
