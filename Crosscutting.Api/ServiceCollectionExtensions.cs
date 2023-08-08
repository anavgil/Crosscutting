using Croscutting.Common.Configurations.Exception;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Crosscutting.Api
{
    public static  class ServiceCollectionExtensions 
    {
        public static IServiceCollection ConfigureBase(this IServiceCollection services)
        {
            services.ConfigureSettings();

            services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

            return services;
        }

        public static IServiceCollection ConfigureSettings(this IServiceCollection services)
        {
            services.ConfigureOptions<ExceptionSettingsBinder>();

            return services;
        }
    }
}