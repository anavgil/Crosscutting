using Croscutting.Common.Configurations.Exception;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.Api
{
    public static  class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureSettings(this IServiceCollection services)
        {
            services.ConfigureOptions<ExceptionSettingsBinder>();

            return services;
        }
    }
}