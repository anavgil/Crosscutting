using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Crosscutting.Base;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureBase(this IServiceCollection services)
    {

        services.AddSerilog();
        return services;
    }


}
