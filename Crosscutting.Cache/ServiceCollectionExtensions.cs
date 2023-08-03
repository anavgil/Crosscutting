using Croscutting.Common.Configurations.Redis;
using Crosscutting.Cache.Abstraction;
using Crosscutting.Cache.Implementation;
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


            services.AddScoped<ICacheService, CacheService>();

            return services;
        }
    }
}