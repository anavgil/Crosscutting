using Croscutting.Common.Configurations.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Crosscutting.Cache
{
    public static  class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCache(this IServiceCollection services, IOptions<RedisSettingsBinder> settings)
        {
            services.ConfigureOptions<RedisOptionsSettingsSetup>();

            
            var redisSettings = settings.Value;

            if(redisSettings.UseRedis)
            {

                var configurationOptions = new ConfigurationOptions
                {
                    EndPoints = { redisSettings.RedisConnectionString },
                    Ssl = redisSettings.UseSSL,
                    Password = redisSettings.RedisToken,
                    ClientName = redisSettings.InstanceName,
                };

                services.AddStackExchangeRedisCache(options =>
                {
                    options.ConfigurationOptions = configurationOptions;
                });
            }
            else
            {
                services.AddDistributedMemoryCache(option =>
                {
                });
            }


            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = new ConfigurationOptions()
                {

                };
            });


            return services;
        }
    }
}