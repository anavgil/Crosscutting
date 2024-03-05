using Croscutting.Common.Configurations.Redis;
using Croscutting.Common.Configurations.Serilog;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Crosscutting.Base.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBase(this IServiceCollection services)
    {
        services.ConfigureSettings();
        services.AddHttpClient();
        services.AddSerilog();

        return services;
    }

    public static IServiceCollection ConfigureSettings(this IServiceCollection services)
    {
        services.ConfigureOptions<SerilogOptionsSettingsSetup>();

        return services;
    }
}
