using Carter;
using Croscutting.Common.Configurations.Exception;
using Microsoft.Extensions.DependencyInjection;

namespace Crosscutting.Api
{
    public static  class ServiceCollectionExtensions 
    {
        public static IServiceCollection ConfigureBase(this IServiceCollection services)
        {
            services.ConfigureSettings();

            //services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
            
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
            
            services.AddControllers();
            services.AddCarter();

            services.AddHealthChecks();
            services.AddHealthChecksUI();

            return services;
        }

        public static IServiceCollection ConfigureSettings(this IServiceCollection services)
        {
            services.ConfigureOptions<ExceptionSettingsBinder>();

            return services;
        }
    }
}