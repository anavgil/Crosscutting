﻿using Croscutting.Common.Configurations.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Crosscutting.Cache
{
    public static  class ServiceCollectionExtensions
    {
        //var configurationOptions = new ConfigurationOptions
        //{
        //    EndPoints = { "finhavadev.redis.cache.windows.net:6379" }, // Unlike aioredis, we don't need to specify "redis://" here
        //    Ssl = false, // Set this to true if your Redis instance can handle connection using SSL
        //    Password = "y3lMlNq8Ih4V8XvyShweljWjpvDkavKRdAzCaJsfqoQ="
        //};

        

        public static IServiceCollection AddCache(this IServiceCollection services, IOptions<RedisSettingsBinder> settings)
        {
            services.ConfigureOptions<RedisOptionsSettingsSetup>();

            
            var redisSettings = settings.Value;

            if(redisSettings.UseRedis)
            {

            }
            else
            {

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