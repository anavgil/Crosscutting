using Carter;
using Croscutting.Common.Configurations.Exception;
using FluentValidation;
using HealthChecks.ApplicationStatus.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;
using System.Threading.RateLimiting;

namespace Crosscutting.Api;
public static  class ServiceCollectionExtensions 
{
    public static IServiceCollection ConfigureBase(this IServiceCollection services)
    {
        services.ConfigureSettings();

        //services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        services.AddRateLimiter(_ =>
        {
            _.OnRejected = (context, _) =>
            {
                if(context.Lease.TryGetMetadata(MetadataName.RetryAfter,out var retryAfter))
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
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly))
                .AddCarterDependencies()
                .AddHealthCheckDependencies()
                .AddCors();

        return services;
    }

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
        services.AddHealthChecksUI();
                //.AddInMemoryStorage();

        return services;
    }

    public static IServiceCollection ConfigureSettings(this IServiceCollection services)
    {
        services.ConfigureOptions<ExceptionSettingsBinder>();

        return services;
    }

    private static IServiceCollection AddCarterDependencies(this IServiceCollection services)
    {
        services.AddCarter(configurator: c =>
        {
            c.WithModules(GetCarterModules());
            c.WithValidators(GetCarterValidation());
        });

        return services;
    }

    private static Type[] GetCarterModules()
    {
        var assembly = Assembly.GetEntryAssembly();

        return assembly.GetTypes()
                    .Where(t =>
                            !t.IsAbstract &&
                            typeof(ICarterModule).IsAssignableFrom(t) &&
                            t != typeof(ICarterModule) &&
                            t.IsPublic
                        ).ToArray();
    }

    private static Type[] GetCarterValidation()
    {
        var assembly = Assembly.GetEntryAssembly();
        var applicationAssembly = assembly.GetReferencedAssemblies()
                                    .Where(x => x.FullName.Contains("Application"))
                                    .FirstOrDefault();

        if (applicationAssembly is null)
            return [];

        return Assembly.Load(applicationAssembly).GetTypes()
                .Where(t => !t.GetTypeInfo().IsAbstract && typeof(IValidator).IsAssignableFrom(t))
                .ToArray();
    }
}
