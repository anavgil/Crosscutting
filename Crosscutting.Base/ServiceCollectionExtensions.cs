using Croscutting.Common.Configurations.Redis;
using Croscutting.Common.Configurations.Serilog;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Crosscutting.Base;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureBase(this IServiceCollection services)
    {
        services.ConfigureSettings();
        services.AddHttpClient();
        services.AddSerilog();

        // Enable exception handling middleware
        services.AddScoped<ExceptionHandlerExtensions>();

        return services;
    }

    public static IServiceCollection ConfigureSettings(this IServiceCollection services)
    {
        services.ConfigureOptions<SerilogOptionsSettingsSetup>();
        services.ConfigureOptions<RedisSettingsBinder>();

        return services;
    }
}
