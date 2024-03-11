using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Crosscutting.Base.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationCrosscutting(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddMapsterConfiguration()
                .AddMapster();

        return services;
    }


    private static IServiceCollection AddMapsterConfiguration(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        var assembly = Assembly.GetEntryAssembly();
        var applicationAssembly = assembly.GetReferencedAssemblies()
                                    .Where(x => x.FullName.Contains("Application"))
                                    .FirstOrDefault();

        if (applicationAssembly is not null)
            config.Scan(Assembly.Load(applicationAssembly));


        return services;
    }

    private static IServiceCollection AddMapsterConfiguration(this IServiceCollection services, Assembly configurationAssembly)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        if (configurationAssembly is not null)
            config.Scan(configurationAssembly);

        return services;
    }

    private static IServiceCollection AddMapsterConfiguration(this IServiceCollection services, string configurationAssemblyName)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        if (!string.IsNullOrWhiteSpace(configurationAssemblyName))
        {
            var assembly = Assembly.GetEntryAssembly();
            var applicationAssembly = assembly.GetReferencedAssemblies()
                                        .Where(x => x.FullName.Contains(configurationAssemblyName))
                                        .FirstOrDefault();

            if (applicationAssembly is not null)
                config.Scan(Assembly.Load(applicationAssembly));
        }


        return services;
    }
}
