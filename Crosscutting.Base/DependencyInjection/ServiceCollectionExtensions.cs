﻿using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Crosscutting.Base.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationCrosscutting(this IServiceCollection services, Assembly configurationAssembly)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddMapsterConfiguration(configurationAssembly)
                .AddMapster();

        return services;
    }

    private static IServiceCollection AddMapsterConfiguration(this IServiceCollection services, Assembly configurationAssembly)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        if (configurationAssembly is not null)
            config.Scan(configurationAssembly);

        return services;
    }
}
